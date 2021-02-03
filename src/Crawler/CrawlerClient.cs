using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Crawler
{
    /// <summary>
    /// 
    /// </summary>
    public class CrawlerClient
    {
        private string url = "https://hq.smm.cn/copper/category/201102250457";

        public async Task Run()
        {
            await CrawlingWeb();
        }

        //HtmlAgilityPack
        private async Task CrawlingWebFunc1()
        {
            var web = new HtmlWeb();
            var htmlDocument = await web.LoadFromWebAsync(url);
            var priceNode = htmlDocument.DocumentNode.SelectSingleNode(
                    "/html/body/div[1]/div[2]/div[1]/div/div[1]/div/div[1]/p[2]/span[2]");

            var changeNode =
                htmlDocument.DocumentNode.SelectSingleNode(
                    "/html/body/div[1]/div[2]/div[1]/div/div[1]/div/div[1]/p[2]/span[3]/span[1]");

            var minNode =
                htmlDocument.DocumentNode.SelectSingleNode(
                    "/html/body/div[1]/div[2]/div[1]/div/div[1]/div/div[1]/p[2]/span[1]/text()[1]");
            var maxNode =
                htmlDocument.DocumentNode.SelectSingleNode(
                    "/html/body/div[1]/div[2]/div[1]/div/div[1]/div/div[1]/p[2]/span[1]/text()[2]");

            var dateNode =
                htmlDocument.DocumentNode.SelectSingleNode(
                    "/html/body/div[1]/div[2]/div[1]/div/div[1]/div/div[1]/p[3]/span");

            var rateNode = htmlDocument.DocumentNode.SelectSingleNode(
                "/html/body/div[1]/div[2]/div[1]/div/div[1]/div/div[1]/p[2]/span[3]/span[3]");

            Console.WriteLine($"均价：{priceNode.InnerText.Trim()}");
            Console.WriteLine($"涨幅：{changeNode.InnerText.Trim()}");
            Console.WriteLine($"最低值：{minNode.InnerText.Trim()}");
            Console.WriteLine($"最高值：{maxNode.InnerText.Trim()}");

            var dateStr = dateNode.InnerText.Trim();
            dateStr = dateStr.Substring(dateStr.Length - 10, 10);
            Console.WriteLine($"更新日期：{dateStr}");

            var rate = decimal.Parse(rateNode.InnerText.Trim().Replace("%", "")) / 100M;
            Console.WriteLine($"涨幅率：{rate}");
        }

        private async Task CrawlingWeb()
        {
            var web = new HtmlWeb();
            var htmlDocument = await web.LoadFromWebAsync(url);
            var allNodes = htmlDocument.DocumentNode;

            StringBuilder sb = new StringBuilder();

            #region 更新日期

            var dateNode = allNodes.SelectSingleNode(
                "/html/body/div[1]/div[2]/div[1]/div/div[1]/div/div[1]/p[3]/span");
            if (dateNode == null)
            {
                sb.AppendLine($"页面Html中未获取到[更新日期]节点");
                Console.WriteLine(sb.ToString());
                return;
            }
            var dateStr = dateNode.InnerText.Trim();
            dateStr = dateStr.Substring(dateStr.Length - 10, 10);
            sb.AppendLine($"更新日期：{dateStr}");

            #endregion

            #region 均价

            var priceNode = allNodes.SelectSingleNode(
                "/html/body/div[1]/div[2]/div[1]/div/div[1]/div/div[1]/p[2]/span[2]");
            if (priceNode == null)
            {
                sb.AppendLine($"页面Html中未获取到[均价]节点");
                Console.WriteLine(sb.ToString());
                return;
            }

            var priceStr = priceNode.InnerText.Trim();
            sb.AppendLine($"均价：{priceStr}");

            #endregion

            #region 最低值

            var minNode = allNodes.SelectSingleNode(
                "/html/body/div[1]/div[2]/div[1]/div/div[1]/div/div[1]/p[2]/span[1]/text()[1]");
            if (minNode == null)
            {
                sb.AppendLine($"页面Html中未获取到[最低值]节点");
                Console.WriteLine(sb.ToString());
                return;
            }

            var minStr = minNode.InnerText.Trim();
            sb.AppendLine($"最低值：{minStr}");

            #endregion

            #region 最高值

            var maxNode = allNodes.SelectSingleNode(
                "/html/body/div[1]/div[2]/div[1]/div/div[1]/div/div[1]/p[2]/span[1]/text()[2]");
            if (maxNode == null)
            {
                sb.AppendLine($"页面Html中未获取到[最高值]节点");
                Console.WriteLine(sb.ToString());
                return;
            }

            var maxStr = maxNode.InnerText.Trim();
            sb.AppendLine($"最高值：{maxStr}");

            #endregion

            #region 涨幅

            var changeNode = allNodes.SelectSingleNode(
                "/html/body/div[1]/div[2]/div[1]/div/div[1]/div/div[1]/p[2]/span[3]/span[1]");
            if (changeNode == null)
            {
                sb.AppendLine($"页面Html中未获取到[涨幅]节点");
                Console.WriteLine(sb.ToString());
                return;
            }

            var changeStr = changeNode.InnerText.Trim();
            sb.AppendLine($"涨幅：{changeStr}");

            #endregion

            #region 增长率

            var rateNode = allNodes.SelectSingleNode(
                "/html/body/div[1]/div[2]/div[1]/div/div[1]/div/div[1]/p[2]/span[3]/span[3]");
            if (rateNode == null)
            {
                sb.AppendLine($"页面Html中未获取到[增长率]节点");
                Console.WriteLine(sb.ToString());
                return;
            }

            var rateStr = rateNode.InnerText.Trim();
            sb.AppendLine($"增长率：{rateStr}");

            #endregion

            Console.WriteLine(sb.ToString());

            Console.WriteLine("11111");
            Console.WriteLine("22222");
            Console.Write("\n");
            Console.WriteLine("33333");
        }


        //Selenium
        private void CrawlingWebFunc()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--test-type", "--ignore-certificate-errors");
            options.AddArgument("enable-automation");


            using (IWebDriver driver = new ChromeDriver(service, options, TimeSpan.FromSeconds(120)))
            {
                driver.Url = url;

                Thread.Sleep(5000);

                //driver.Navigate().GoToUrl(url);
                var responseModel = driver.PageSource;
                Console.WriteLine(responseModel);
            }
        }

        /*
        private void crawlingWebFunc()
        {
            SetText("\r\n开始尝试...");
            List<testfold> surls = new List<testfold>();
            string path = System.Environment.CurrentDirectory + "\\图片url\\";
            DirectoryInfo root = new DirectoryInfo(path);
            DirectoryInfo[] dics = root.GetDirectories();
            foreach (var itemdic in dics)
            {
                string txt = "";
                StreamReader sr = new StreamReader(itemdic.FullName + "\\data.txt");
                while (!sr.EndOfStream)
                {
                    string str = sr.ReadLine();
                    txt += str;// + "\n";
                }
                sr.Close();
                surls.Add(new testfold() { key = itemdic.FullName, picurl = txt });
            }

            ChromeDriverService service = ChromeDriverService.CreateDefaultService(System.Environment.CurrentDirectory);
            //  service.HideCommandPromptWindow = true;

            ChromeOptions options = new ChromeOptions();
            options.AddArguments("--test-type", "--ignore-certificate-errors");
            options.AddArgument("enable-automation");
            //   options.AddArgument("headless");
            //  options.AddArguments("--proxy-server=http://user:password@yourProxyServer.com:8080");

            using (IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(service, options, TimeSpan.FromSeconds(120)))
            {
                driver.Url = "https://www.1688.com/";
                Thread.Sleep(200);
                try
                {
                    int a = 1;
                    foreach (var itemsurls in surls)
                    {
                        SetText("\r\n第" + a.ToString() + "个");
                        driver.Navigate().GoToUrl(itemsurls.picurl);
                        //登录
                        if (driver.Url.Contains("login.1688.com"))
                        {
                            SetText("\r\n需要登录，开始尝试...");
                            trylogin(driver); //尝试登录完成
                                              //再试试
                            driver.Navigate().GoToUrl("https://s.1688.com/youyuan/index.htm?tab=imageSearch&imageType=oss&imageAddress=cbuimgsearch/eWXC7XHHPN1607529600000&spm=");

                            if (driver.Url.Contains("login.1688.com"))
                            {
                                //没办法退出
                                SetText("\r\n退出，换ip重试...");
                                return;
                            }
                        }

                        //鼠标放上去的内容因为页面自带只能显示一个的原因 没办法做到全部显示 然后在下载 只能是其他方式下载
                        //  var elements = document.getElementsByClassName('hover-container');
                        //  Array.prototype.forEach.call(elements, function(element) {
                        //  element.style.display = "block";
                        //   console.log(element);
                        //  });

                        //   IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                        //    var sss = js.ExecuteScript(" var elements = document.getElementsByClassName('hover-container');  Array.prototype.forEach.call(elements, function(element) {  console.log(element); element.setAttribute(\"class\", \"测试title\");  element.style.display = \"block\";  console.log(element); });");

                        Thread.Sleep(500);
                        var responseModel = Write(itemsurls.key, driver.PageSource, Pagetypeenum.列表);
                        Thread.Sleep(500);
                        int i = 1;
                        foreach (var offer in responseModel?.data?.offerList ?? new List<OfferItemModel>())
                        {
                            driver.Navigate().GoToUrl(offer.information.detailUrl);
                            string responseDatadetail = driver.PageSource;
                            Write(itemsurls.key, driver.PageSource, Pagetypeenum.详情);
                            SetText("\r\n第" + a.ToString() + "-" + i.ToString() + "个");
                            Thread.Sleep(500);
                            i++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    CloseChromeDriver(driver);
                    throw;
                }
            }
        }

        */

    }
}
