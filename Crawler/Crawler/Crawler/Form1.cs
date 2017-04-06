using CrawlerDll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;

namespace Crawler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            init_weibo();
        }

        #region weibo 没登录要隔一段时间会搜出来不一样。。要登陆！你给我账号我就写模拟登陆！

        void init_weibo()
        {
            textBox_keyword.Text = "一个人的电影";
            textBox_maxthreadcnt.Text = "400";
            textBox_period.Text = "5000";//感觉随便写几都会被当机器人 可能要改个随机数，只试过1和1000
        }

        bool Is_weibo_Alive = false;

        int weibo_cnt_all = 0;
        int weibo_cnt_current = 0;
        int weibo_cnt_succeed = 0;
        int weibo_cnt_err = 0;
        int weibo_cnt_err1 = 0;
        int weibo_cnt_err2 = 0;
        int weibo_cnt_err3 = 0;
        int weibo_cnt_err4 = 0;
        int weibo_cnt_insert = 0;
        int weibo_cnt_repeat = 0;

        enum Weibo_thread_state
        {
            处理中,
            处理成功, 处理错误未找到有效内容, 处理错误匹配数量有误, 处理错误sql错误, 处理错误数据异常如机器人验证,
            找到数据, 已有重复数据
        }

        class Weibo_thread
        {
            public Weibo_thread_state state { get; set; }
        }

        class Weibo_input
        {
            public Weibo_input(string keyword, string maxthreadcnt, string period)
            {
                this.keyword = keyword;
                this.maxthreadcnt = int.Parse(maxthreadcnt);
                this.period = int.Parse(period);
            }
            public string keyword { get; set; }
            public int maxthreadcnt { get; set; }
            public int period { get; set; }
        }

        private void button_weibo_Click(object sender, EventArgs e)
        {
            if (Is_weibo_Alive)
            {
                Is_weibo_Alive = false;
                button_weibo.Text = "开始";
            }
            else
            {
                if (weibo_cnt_current == 0)
                {
                    Is_weibo_Alive = true;
                    button_weibo.Text = "暂停";

                    weibo_cnt_all = 0;
                    weibo_cnt_current = 0;
                    weibo_cnt_succeed = 0;
                    weibo_cnt_err = 0;
                    weibo_cnt_err1 = 0;
                    weibo_cnt_err2 = 0;
                    weibo_cnt_err3 = 0;
                    weibo_cnt_err4 = 0;
                    weibo_cnt_insert = 0;
                    weibo_cnt_repeat = 0;

                    var t = new Thread(new ParameterizedThreadStart(Thread_weibo));
                    t.IsBackground = true;
                    t.Start(new Weibo_input(textBox_keyword.Text, textBox_maxthreadcnt.Text, textBox_period.Text));
                }
            }
        }

        void Thread_weibo(object obj)
        {
            var _weibo_input = obj as Weibo_input;

            while (true)
            {
                if (weibo_cnt_current <= _weibo_input.maxthreadcnt)
                {
                    var t = new Thread(new ParameterizedThreadStart(Thread_weibo_each));
                    t.IsBackground = true;
                    t.Start(_weibo_input.keyword);
                }

                Thread.Sleep(_weibo_input.period);

                if (!Is_weibo_Alive)
                {
                    break;
                }
            }
        }

        delegate void InvokeDelegate(Weibo_thread _weibo_thread);

        void SetLabel_weibostateWithInvoke(Weibo_thread _weibo_thread)
        {
            if (this.InvokeRequired)
            {
                var _InvokeDelegate = new InvokeDelegate(SetLabel_weibostate);
                this.Invoke(_InvokeDelegate, new object[] { _weibo_thread });
            }
            else
            {
                SetLabel_weibostate(_weibo_thread);
            }
        }

        void SetLabel_weibostate(Weibo_thread _weibo_thread)
        {
            switch (_weibo_thread.state)
            {
                case Weibo_thread_state.处理中:
                    weibo_cnt_all++;
                    weibo_cnt_current++;
                    break;
                case Weibo_thread_state.处理成功:
                    weibo_cnt_current--;
                    weibo_cnt_succeed++;
                    break;
                case Weibo_thread_state.处理错误未找到有效内容:
                    weibo_cnt_current--;
                    weibo_cnt_err++;
                    weibo_cnt_err1++;
                    break;
                case Weibo_thread_state.处理错误匹配数量有误:
                    weibo_cnt_current--;
                    weibo_cnt_err++;
                    weibo_cnt_err2++;
                    break;
                case Weibo_thread_state.处理错误sql错误:
                    weibo_cnt_current--;
                    weibo_cnt_err++;
                    weibo_cnt_err3++;
                    break;
                case Weibo_thread_state.处理错误数据异常如机器人验证:
                    weibo_cnt_current--;
                    weibo_cnt_err++;
                    weibo_cnt_err4++;
                    break;
                case Weibo_thread_state.找到数据:
                    weibo_cnt_insert++;
                    break;
                case Weibo_thread_state.已有重复数据:
                    weibo_cnt_repeat++;
                    break;
            }

            label_weibostate.Text = "已开启线程总数:" + weibo_cnt_all + ";"
                + "处理中:" + weibo_cnt_current
                + "\n"
                + "错误:" + weibo_cnt_err + "(处理错误未找到有效内容 " + weibo_cnt_err1 + ",处理错误匹配数量有误 " + weibo_cnt_err2 + ",处理错误sql错误 " + weibo_cnt_err3 + ",处理错误数据异常如机器人验证 " + weibo_cnt_err4 + ")"
                + "\n"
                + "找到数据:" + weibo_cnt_insert
                + "已有重复数据:" + weibo_cnt_repeat;
        }

        void Thread_weibo_each(object obj)
        {
            string keyword = obj as string;

            var _weibo_thread = new Weibo_thread();
            _weibo_thread.state = Weibo_thread_state.处理中;
            SetLabel_weibostateWithInvoke(_weibo_thread);

            var str = Uri.EscapeDataString(keyword);
            var url = "http://s.weibo.com/weibo/" + str;

            string content = Methods.Read(url);

            //获取内容
            var _List_script = Methods.GetData(content, "script", string.Empty
                , @"<script>STK && STK.pageletM && STK.pageletM.view("
                , @")</script>");

            string data = string.Empty;

            //只保留搜索内容
            foreach (var item in _List_script)
            {
                if (item.IndexOf(@"""pl_weibo_direct""") != -1)
                {
                    data = item;
                }
            }

            var _l_name = new List<string>();
            var _l_content = new List<string>();
            var _l_url = new List<string>();

            if (string.IsNullOrEmpty(data))
            {
                _weibo_thread.state = Weibo_thread_state.处理错误数据异常如机器人验证;
                SetLabel_weibostateWithInvoke(_weibo_thread);
            }
            else
            {
                var str_script = Methods.ReadFromJson(data, "html");

                if (!string.IsNullOrEmpty(str_script))
                {
                    var _List_p = Methods.GetData(str_script, "p", "comment_txt"
                        , @"nick-name="""
                        , @"</p>");

                    foreach (var item_p in _List_p)
                    {
                        int index = item_p.IndexOf(@""">");

                        _l_name.Add(item_p.Substring(0, index));
                        _l_content.Add(item_p.Substring(index + 2));
                    }

                    var _List_url = Methods.GetData(str_script, "div", "feed_from W_textb"
                        , @"href="""
                        , @""" target");

                    foreach (var item_url in _List_url)
                    {
                        _l_url.Add(item_url);
                    }
                }

                if (_l_name.Count == _l_content.Count && _l_content.Count == _l_url.Count)
                {
                    if (_l_name.Count != 0)
                    {
                        string connStr = "Data Source=.;Initial Catalog=data;User ID=sa;Pwd=sasasa;Connect Timeout=2;";
                        var sqlconn = new SqlConnection(connStr);
                        var sqlcommand = new SqlCommand();
                        sqlcommand.CommandType = CommandType.Text;
                        sqlcommand.Connection = sqlconn;

                        try
                        {
                            sqlconn.Open();

                            for (int i = 0; i < _l_name.Count; i++)
                            {
                                sqlcommand.CommandText = string.Format("select 1 from data..weibo(nolock) where url = '{0}' and keyword = '{1}'", _l_url[i], keyword);
                                var dr = sqlcommand.ExecuteReader();
                                bool has_record = false;
                                while (dr.Read())
                                {
                                    has_record = dr[0].ToString() == "1";
                                }
                                dr.Close();

                                if (!has_record)
                                {
                                    sqlcommand.CommandText = string.Format("insert into data..weibo(name,content,url,keyword)values('{0}','{1}','{2}','{3}')", _l_name[i], _l_content[i], _l_url[i], keyword);
                                    sqlcommand.ExecuteNonQuery();

                                    _weibo_thread.state = Weibo_thread_state.找到数据;
                                    SetLabel_weibostateWithInvoke(_weibo_thread);
                                }
                                else
                                {
                                    _weibo_thread.state = Weibo_thread_state.已有重复数据;
                                    SetLabel_weibostateWithInvoke(_weibo_thread);
                                }
                            }

                            sqlcommand.Dispose();
                            sqlconn.Close();

                            _weibo_thread.state = Weibo_thread_state.处理成功;
                            SetLabel_weibostateWithInvoke(_weibo_thread);
                        }
                        catch (Exception ex)
                        {
                            _weibo_thread.state = Weibo_thread_state.处理错误sql错误;
                            SetLabel_weibostateWithInvoke(_weibo_thread);
                        }
                    }
                    else
                    {
                        _weibo_thread.state = Weibo_thread_state.处理错误未找到有效内容;
                        SetLabel_weibostateWithInvoke(_weibo_thread);
                    }
                }
                else
                {
                    _weibo_thread.state = Weibo_thread_state.处理错误匹配数量有误;
                    SetLabel_weibostateWithInvoke(_weibo_thread);
                }
            }
        }

        #endregion

        #region haiguan 突然要登陆 还要图片验证码了 我是上的www.haiguan.info 晚点找你

        #endregion
    }
}