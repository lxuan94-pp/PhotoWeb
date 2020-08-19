﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using User.Entity;
namespace PhotoWeb.Controllers
{
    public class UserController : ApiController
    {
        
        UserService myUserDBService = new UserService();
        PhotoService myPhotoService = new PhotoService();

        //注册账户
        [Route("api/User/signup")]
        [HttpPost]
        public String signUP(User.Entity.User loginUser)
        {
            if (loginUser.name==null || loginUser.passWord==null || loginUser.email==null)
            {
                return "三项均不能为空";
            }
            bool[] isVaild = myUserDBService.register(loginUser.name, loginUser.passWord, loginUser.email);
            if (isVaild[0] == false)
            {
                //账号重复
                return "账户名已被注册";
            }
            if (isVaild[1] == false)
            {
                //邮箱重复
                return "邮箱已被注册";
            }
            //成功注册账户
            myUserDBService.signUP(loginUser.name, loginUser.passWord, loginUser.email);
            return "注册成功";
        }

        //登陆账户
        [Route("api/User/login")]
        [HttpPost]
        public String login(User.Entity.User loginUser)
        {
            bool b = myUserDBService.login(loginUser.name, loginUser.passWord);
            if (b == true)
            {
                return "登陆成功";
            }
            else
            {
                return "账户或密码错误";
            }
        }

        //获取图片
        [Route("api/User/get")]
        [HttpPost]
        public String[] get(User.Entity.User loginUser)
        {   String[] srcArray= myPhotoService.getPhotoByUser(loginUser.name);
            return srcArray;
        }

        //删除图片
        [Route("api/User/del")]
        [HttpPost]
        public void delete(Photo myPhoto)
        {
            myPhotoService.deleteOne(myPhoto);
        }


        //异步保存照片
        [Route("api/User/upload")]
        [HttpPost]
        public async Task<Dictionary<string, string>> Post(String myUser="guest")
        {
            //如果文件格式不是Multipart
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            
            string root = HttpContext.Current.Server.MapPath("~/Data/");//指定要将文件存入的服务器物理位置
            var provider = new MultipartFormDataStreamProvider(root);


            //返回值
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //录入数据库的图片信息
            Dictionary<String, String> photo = new Dictionary<string, string>();

            try
            {
                //异步获取图片
                await Request.Content.ReadAsMultipartAsync(provider);
               
                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {//接收文件
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);//获取上传文件实际的文件名
                    Trace.WriteLine("Server file path: " + file.LocalFileName);//获取上传文件在服务上默认的文件名

                    //获取图片本地名称，从数据库中对比，看是否重名
                    string subAddress="";
                    Random fileN;
                    string filename="";
                    string extension = "jpg";
                    do
                    {
                        subAddress = myUser;
                        fileN = new Random(DateTime.Now.Hour + DateTime.Now.Millisecond);
                        filename = fileN.Next(99999).ToString();
                    }
                    while (myPhotoService.isExited(subAddress + "/" + filename + "." + extension));//如果重名了就重复随机名称           
                    
                    String AppPath= System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    //保存至本地                
                    string destFile = AppPath+ "/Data/" + subAddress +"/"+ filename + "." + extension;
                    DirectoryInfo dir = new DirectoryInfo(AppPath+ "/Data/" + subAddress);
                    //不存在创建文件夹
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    File.Move(file.LocalFileName, destFile);


                    //录入数据库      
                    photo.Add( subAddress + "/" + filename + "." + extension, myUser); //key为地址，value为用户名
                }

                //数据库
                  myPhotoService.insert(photo);
                foreach (var key in provider.FormData.AllKeys)
                {
                    //接收FormData
                    dic.Add(key, provider.FormData[key]);
                }
            }
            catch
            {
                throw;
            }
            return dic;
        }




    }
}
