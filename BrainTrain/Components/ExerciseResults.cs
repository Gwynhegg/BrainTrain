using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace BrainTrain.Components
{
    public class ExerciseResults
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int points { get; set; }

        public DateTime date { get; set; }
    }

    public class memoryTable : ExerciseResults { }

    public class memoryText : ExerciseResults { }

    public class fastCalculating : ExerciseResults { }

    public class fastSummarize : ExerciseResults { }

    public class attentionSwitch : ExerciseResults { }

    public class colorAttention : ExerciseResults { }

}
