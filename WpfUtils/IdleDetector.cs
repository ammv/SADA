using System;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace WpfUtils
{
    internal struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }

    /// <summary>
    /// Временная информация о бездействии пользователя
    /// </summary>
    public class IdleTimeInfo
    {
        /// <summary>
        /// Время бездействия
        /// </summary>
        public TimeSpan IdleTime { get; internal set; }
        /// <summary>
        /// Время последней активности
        /// </summary>
        public DateTime LastInputTime { get; internal set; }
        /// <summary>
        /// Системное время
        /// </summary>
        public int SystemUptimeMilliseconds { get; internal set; }
    }

    /// <summary>
    /// Детектор бездействия пользователя. Обнаруживает бездействие пользователя в приложении и вне его (Свернул его и ушёл)
    /// </summary>
    public class IdleDetector
    {
        private DispatcherTimer _timer;
        private TimeSpan _idleDetectInterval;
        private TimeSpan _checkInterval;

        /// <summary>
        /// Время бездействия пользователя для детекции.
        /// Если время бездействия пользователя больше или равно этому значению, то вызывается событие IdleDetect
        /// </summary>
        public TimeSpan IdleDetectInterval 
        { 
            get => _idleDetectInterval;
            set => _idleDetectInterval = value;
        }
        /// <summary>
        /// Интервал проверки бездействия. Оптимально 1 секунда
        /// Т.е. каждую секунду будет проверяться текущее время бездействия 
        /// </summary>
        public TimeSpan CheckInterval
        { 
            get => _checkInterval;
            set
            {
                _checkInterval = value;
                if(_timer.IsEnabled)
                {
                    _timer.IsEnabled = false;
                    _timer.Interval = _checkInterval;
                    _timer.IsEnabled = true;
                }
                else
                {
                    _timer.Interval = _checkInterval;
                }
            } 
        }

        /// <summary>
        /// Создает экземплятор детектора бездействия с заданными параметрами
        /// </summary>
        /// <param name="idleDetectInterval">Время бездействия пользователя для детекции</param>
        /// <param name="checkInterval">Интервал проверки бездействия. Оптимально 1 секунда</param>
        public IdleDetector(TimeSpan idleDetectInterval, TimeSpan checkInterval)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = checkInterval;
            _timer.Tick += IdleDetector_Tick;
            _idleDetectInterval = idleDetectInterval;
            _checkInterval = checkInterval;
        }

        public delegate void IdleDetectHandler(IdleDetector sender, IdleTimeInfo idleTimeInfo);

        /// <summary>
        /// Событие возникающее когда время бездействия пользователя больше или равно IdleDetectInterval
        /// </summary>
        public event IdleDetectHandler IdleDetect;

        /// <summary>
        /// Запускает детектор
        /// </summary>
        public void Start()
        {
            _timer.Start();
        }

        /// <summary>
        /// Останавливает детектор
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        private static IdleTimeInfo GetIdleTimeInfo()
        {
            int systemUptime = Environment.TickCount,
                lastInputTicks = 0,
                idleTicks = 0;

            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                lastInputTicks = (int)lastInputInfo.dwTime;

                idleTicks = systemUptime - lastInputTicks;
            }

            return new IdleTimeInfo
            {
                LastInputTime = DateTime.Now.AddMilliseconds(-1 * idleTicks),
                IdleTime = new TimeSpan(0, 0, 0, 0, idleTicks),
                SystemUptimeMilliseconds = systemUptime,
            };
        }

        private void IdleDetector_Tick(object sender, EventArgs e)
        {
            var info = GetIdleTimeInfo();

            if (info.IdleTime >= _idleDetectInterval)
            {
                IdleDetect?.Invoke(this, info);
            }
        }
    }
}