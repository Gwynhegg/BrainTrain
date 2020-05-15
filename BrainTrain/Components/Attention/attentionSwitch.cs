using System;
using Xamarin.Forms;


namespace BrainTrain.Components.Attention
{

    //Вспомогательный статический класс, отвечающий за получение случайного изображения из коллекции локальнх ресурсов Android.
    public static class IFigure
    {
        //Главный метод данного класса.
        public static ImageSource getRandomPic(Image img)
        {
            Random rnd = new Random();
            Image temp = new Image();

            //С определенной вероятностью выдает ссылку на то же самое изображение.
            if (rnd.Next(1, 6) == 1 && img.Source!=null) return img.Source; else
            {
                //Получение ссылки на источник случайной картинки из локального расположения.
                ImageSource source =  ImageSource.FromResource("BrainTrain.Images.img"+rnd.Next(9)+".png");
                return source;
            }
        }
    }

    //Задание на смену внимания. Перед игроком будут последовательно показываться картинки. Его цель - определить, совпадает ли текущая картинка с предыдущей.
    //Класс наследывается от абстрактного класса Exercise
    class attentionSwitch : Exercise
    {
        //Переменная, хранящая ссылку предыдущего изображения для сравнения с предыдущими.
        private ImageSource  previous_image;

        //Устанавливаем источник изображения в качестве предыдущего.
        public void setPrevImage(ImageSource img)
        {
            previous_image = img;
        }

        //Конструктор класса. С помощью ключевого слова base обращаемся к классу-предку.
        public attentionSwitch(ref Grid grid) : base(ref grid)
        {
            //Устанавливаем описание задания для отображения на форме.
            description = "На экране последовательно будут появляться фигуры. \n Ваша задача: определить, сопадает ли текущая фигура с предыдущей.";
        }

        //Метод проверки правильности ответа. Логический параметр - ответ, который дал пользователь (Поскольку представлен в формате Да/Нет").
        public void checkLevel(bool ans)
        {
            //Если картинки не совпадают, и ответ Нет, или Если картинки совпадают, и ответ Да, то данный ответ верный.
            if ((((Image)content_grid.FindByName("img_task")).Source==previous_image && ans) || (((Image)content_grid.FindByName("img_task")).Source != previous_image && !ans))
            {
                //Прибавляем очки и меняем сложность в сторону повышения.
                general_points += difficulty;
                current_mistakes = 0;
                if (++current_positives > 2) raiseDifficulty();
            }
            else
            {
                //Меняем сложность в обратную сторону.
                current_positives = 0;
                if (++current_mistakes > 2) lowerDifficulty();
            }
            //Устанавливаем текущий источник в качестве предыдущего.
            setPrevImage(((Image)content_grid.FindByName("img_task")).Source);
            //Собираем мусор.
            GC.Collect();
            //В главном потоке генерируем новый уровень.
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        //Метод, отвечающий за генерацию нового уровня.
        public override void generateLevel()
        {
            //Отображаем измененные компоненты формы.
            displayComponents();
            //Получаем случайную картинку из локальных ресурсов и отображаем на форме.
            Device.BeginInvokeOnMainThread(() => ((Image)content_grid.FindByName("img_task")).Source = IFigure.getRandomPic(((Image)content_grid.FindByName("img_task"))));
            //Сбрасываем таймер и запускаем его.
            progress = 1;
            task_timer.Start();
        }

        //Метод, отвечающий за понижение сложности. Переопределение метода позволяет задать формулу изменения сложности индивидуально для каждого задания.
        public override void lowerDifficulty()
        {
            difficulty -= current_mistakes;
            if (difficulty < 1) difficulty = 1;
        }

        //Метод, отвечающий за повышение сложности.
        public override void raiseDifficulty()
        {
            difficulty += current_positives / 5;
        }

        //Метод, отвечающий за первоначаную задачу параметров и старт уровня.
        public override void startLevel()
        {
           //В данном методе задается интервал, в течение которого будет выполняться задание.
            end_time = DateTime.Now.AddMinutes(1);
            previous_image = ((Image)content_grid.FindByName("img_task")).Source;
            //Запускается главный таймер и генерируется первый уровень.
            task_timer.Enabled = true;
            general_timer.Enabled = true; general_timer.Start();
            Device.BeginInvokeOnMainThread(() => generateLevel());
        }

        //Поскольку данная задача имеет параметр в методе проверки уровня, метод перегружен, а его основоположник переопределен "вхолостую".
        public override void checkLevel() { return;  }

    }
}
