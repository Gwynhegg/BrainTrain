using Android.Content.Res;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace BrainTrain.Components
{
    //Абстрактный класс упражнения, от которого наследуются все остальные.
    public abstract class Exercise
    {
        //переменные, отвечающеи за очки, сложность и количество ошибок и правильных ответов.
        protected int general_points, difficulty, current_mistakes, current_positives;
        //Описание текущего задания.
        protected string description;
        protected Random rnd;
        //Главный таймер всего задания и таймер текущего уровня.
        protected System.Timers.Timer general_timer, task_timer;
        //DateTime, отвечающий за окончание кокретного упражнения.
        protected DateTime end_time;
        //Панель, на которую выводится вся необходимая информация.
        protected Grid content_grid;
        protected double progress = 1;
    
        //Конструктор класса.
        public Exercise(ref Grid grid)
        {
            //Инициализация основных переменных и задание обработчиков событий таймеров.
            rnd = new Random();
            content_grid = grid;
            current_mistakes = 0; current_positives = 0; general_points = 0; difficulty = 1;
            general_timer = new System.Timers.Timer() { Enabled = false, Interval=100};
            general_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
            task_timer = new System.Timers.Timer() { Enabled = false, Interval = 10 };
            task_timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTaskEvent);
            end_time = new DateTime();
        }

        //Основное событие таймера уровня.
        protected void OnTaskEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            //Ограничительный таймер текущего уровня. СКорость убывания зависит от сложности.
            progress -= (double)(difficulty) / 1000;
            if (progress < 0)
            {
                //При истечении таймера данный уровень считается проваленным и ошибочным, и генерируется новый уровень.
                task_timer.Stop();
                current_positives = 0;
                if (++current_mistakes > 2) lowerDifficulty();
                Device.BeginInvokeOnMainThread(() => generateLevel());
            }
            Device.BeginInvokeOnMainThread(() => ((ProgressBar)content_grid.FindByName("current_time")).Progress = progress);
        }

        //Событие-обработчик главного таймера всего упражнения.
        protected void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan cur = end_time - e.SignalTime;
            if (cur.TotalMilliseconds <= 0)
            {
                //При окончании отведенного времени игрока переносит на форму с отображением его результата и занесение результата в базу данных.
                general_timer.Dispose();
                Console.WriteLine(this.GetType());
                Device.BeginInvokeOnMainThread(() => Application.Current.MainPage = new Forms.ResultPage(this.GetType().ToString(),general_points));
            }
            Device.BeginInvokeOnMainThread(() => ((Label)content_grid.FindByName("txt_timer")).Text = cur.TotalSeconds.ToString("F1"));
        }

        //Аннулирование таймера для предотвращение работы в отдельных потоках.
        public void timerAnnulate()
        {
            general_timer.Dispose();
        }

        //Метод, отвечающий за первоначаную задачу параметров и старт уровня.
        public abstract void startLevel();

        //Метод, отвечающий за отображение описания задания.
        public string showDescription()
        {
            return description;
        }

        //Метод, отвечающий за повышение сложности.
        public abstract void raiseDifficulty();
        //Метод, отвечающий за понижение сложности.
        public abstract void lowerDifficulty();
        //Метод, отвеачющий за генерацию уровня.
        public abstract void generateLevel();
        //Метод, отвечающий за проверку уровня.
        public abstract void checkLevel();
        //Метод, отвечающий за вывод базовых компонентов.
        protected void displayComponents() 
        {
            ((Label)content_grid.FindByName("txt_difficulty")).Text = "Сложность: \n" + difficulty.ToString();
            ((Label)content_grid.FindByName("txt_points")).Text = "Очки: \n" + general_points.ToString();
        }
    }
}
