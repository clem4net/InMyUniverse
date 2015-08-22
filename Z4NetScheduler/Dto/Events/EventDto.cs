using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Z4NetScheduler.Dto.Events
{
    /// <summary>
    /// Main event class.
    /// </summary>
    [Table("Scheduler_Events")]
    public class EventDto
    {

        /// <summary>
        /// Description.
        /// </summary>
        [Column("DESC"), MaxLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// Event identifier.
        /// </summary>
        [Key, Column("ID"), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Identifier { get; set; }

        /// <summary>
        /// Last time of execution.
        /// </summary>
        [Column("LASTEXECUTION")]
        public DateTime? LastExecution { get; set; }

        /// <summary>
        /// Linked task.
        /// </summary>
        [ForeignKey("TaskIdentifier")]
        public TaskDto Task { get; set; }

        /// <summary>
        /// Task identifier.
        /// </summary>
        [Column("TASK_ID")]
        public int TaskIdentifier { get; set; }

    }
}
