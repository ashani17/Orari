using Orari.DTO.ExamDTO;
using Orari.Models;
using System.ComponentModel.DataAnnotations;

namespace Orari.DTO.ScheduleDTO
{
    public class PutScheduleDTO
    {
        public PutScheduleDTO()
        {
        }
        public PutScheduleDTO(DateOnly date, TimeOnly startTime, TimeOnly endTime, string room, string professor, string course, PutExamDTO postExamDTO)
        {
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            Room = room;
            Profesor = professor;
            Course = course;
            Exam = new Exams
            {
                ExamName = postExamDTO.ExamName,
                ExamDate = postExamDTO.ExamDate,
                StartTime = postExamDTO.StartTime,
                EndTime = postExamDTO.EndTime,
                CId = postExamDTO.CourseId,
                SCId = postExamDTO.ScheduleId,
                PId = postExamDTO.ProfesorId,
                RId = postExamDTO.RoomId
            };
        }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        [Required]
        public string Room { get; set; }
        [Required]
        public string Profesor { get; set; }
        [Required]
        public string Course { get; set; }

        public Exams Exam { get; set; }
    }
}
