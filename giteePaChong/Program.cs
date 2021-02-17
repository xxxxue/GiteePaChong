using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Parser;

namespace giteePaChong
{
    class Program
    {
        private static string _giteeUrl = "https://gitee.com/gvp/all";

        // 欲获取的节点 例子
        //<div class='ui mini label'>Java</div>
        static async Task Main(string[] args)
        {
            var data = await GetHtml(_giteeUrl);
            var htmlParser = new HtmlParser();
            // HTML文本 解析为 HtmlDocument 对象
            var doc = await htmlParser.ParseDocumentAsync(data);
            // 筛选出符合要求的 标签
            var list = doc.All.Where(
                item =>
                    item.ClassName == "ui mini label"
            ).ToList();

            Console.WriteLine($"总数:{list.Count}");

            var langList = new List<string>();

            foreach (var item in list)
            {
                langList.Add(item.InnerHtml);// 提取标签中的 语言名称
            }
            
            var dic = 
                langList.GroupBy(item => item) // 分组
                .OrderByDescending(item=>item.Count()); // 倒序
          
            foreach (var item in dic)
            {
                // 输出
                Console.WriteLine($"{item.Key} {item.Count()}");
            }

            Console.ReadLine();
        }

        /// <summary>
        /// 获取html数据
        /// </summary>
        /// <returns></returns>
        private static async Task<string> GetHtml(string url)
        {
            var httpClient = new HttpClient();
            var htmlContent = await httpClient.GetStringAsync(url);
            return htmlContent; 
        }
    }
}