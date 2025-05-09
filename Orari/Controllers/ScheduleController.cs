﻿using Microsoft.AspNetCore.Mvc;
using Orari.DataDbContext;
using Orari.DTO.ScheduleDTO;
using Orari.Interfaces;
using Orari.Models;
using Orari.Services;

namespace Orari.Controllers
{
    [Route("api/schedule")]
    public class ScheduleController : Controller
    {
        
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
           
            _scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schedules = await _scheduleService.GetAllSchedules();
            return Ok(schedules);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostScheduleDTO schedule)
        {
            if (schedule == null)
            {
                return BadRequest();
            }

            // Map PostScheduleDTO to Schedules model
            var scheduleModel = new Schedules
            {
                Date = schedule.Date,
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                Profesor = schedule.Profesor,
                Room = schedule.Room,
                Course = schedule.Course,
                Exam = schedule.Exam,
            };

            var createdSchedule = await _scheduleService.CreateScheduleAsync(scheduleModel);
            return CreatedAtAction(nameof(GetById), new { id = createdSchedule.SCId }, createdSchedule);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PutScheduleDTO schedule)
        {
            if (schedule == null)
            {
                return BadRequest();
            }
            var existingSchedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (existingSchedule == null)
            {
                return NotFound();
            }
            existingSchedule.Date = schedule.Date;
            existingSchedule.StartTime = schedule.StartTime;
            existingSchedule.EndTime = schedule.EndTime;
            existingSchedule.Profesor = schedule.Profesor;
            existingSchedule.Room = schedule.Room;
            existingSchedule.Course = schedule.Course;
            existingSchedule.Exam = schedule.Exam;


            var updatedSchedule = await _scheduleService.UpdateScheduleAsync(existingSchedule);
            return Ok(updatedSchedule);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _scheduleService.GetScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            await _scheduleService.DeleteScheduleAsync(id);
            return NoContent();
        }

        [HttpGet("profesor/{id}")]
        public async Task<IActionResult> GetByProfesor(int id)
        {
            var schedules = await _scheduleService.GetSchedulesByProfesorAsync(id);
            return Ok(schedules);
        }

        [HttpGet("room/{id}")]
        public async Task<IActionResult> GetByRoom(int id)
        {
            var schedules = await _scheduleService.GetSchedulesByRoomAsync(id);
            return Ok(schedules);
        }

    }
}
