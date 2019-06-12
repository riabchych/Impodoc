using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImpoDoc.Entities
{
    [Table("Executions")]
    public class Execution : BaseEntity<Execution>
    {
        public Execution()
        {
            Executor = new Employee();
        }

        public int? ExecutorId { get; set; }

        [ForeignKey("ExecutorId")]
        public Employee Executor
        {
            get { return GetValue(() => Executor); }
            set { SetValue(() => Executor, value); }
        }

        public string Result
        {
            get { return GetValue(() => Result); }
            set { SetValue(() => Result, value); }
        }

        public DateTime CreatedAt
        {
            get { return GetValue(() => CreatedAt); }
            set { SetValue(() => CreatedAt, value); }
        }

        public DateTime CompletedAt
        {
            get { return GetValue(() => CompletedAt); }
            set { SetValue(() => CompletedAt, value); }
        }
    }
}