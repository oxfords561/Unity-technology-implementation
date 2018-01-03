using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AhpilyServer.Timer
{
    /// <summary>
    /// 当定时器达到时间后的触发
    /// </summary>
    public delegate void TimerDelegate();

    /// <summary>
    /// 定时器任务的数据模型
    /// </summary>
    public class TimerModel
    {
        public int Id;

        /// <summary>
        /// 任务执行的事件
        /// </summary>
        public long Time;

        private TimerDelegate timeDelegate;

        public TimerModel(int id, long time, TimerDelegate td)
        {
            this.Id = id;
            this.Time = time;
            this.timeDelegate = td;
        }

        /// <summary>
        /// 触发任务的
        /// </summary>
        public void Run()
        {
            timeDelegate();
        }

    }
}
