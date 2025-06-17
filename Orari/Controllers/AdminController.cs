using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orari.Models;
using Orari.Repository;
using Orari.Services;
using Orari.Interfaces;
using Orari.DTO.AdminDTO;
using Orari.DTO.CoursesDTO;
using Orari.DTO.ScheduleDTO;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly EnrollmentService _enrollmentService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICourseService _courseService;
        private readonly IScheduleService _scheduleService;
        private readonly IRoomService _roomService;

        public AdminController(
            EnrollmentService enrollmentService,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ICourseService courseService,
            IScheduleService scheduleService,
            IRoomService roomService)
        {
            _enrollmentService = enrollmentService;
            _userManager = userManager;
            _roleManager = roleManager;
            _courseService = courseService;
            _scheduleService = scheduleService;
            _roomService = roomService;
        }

        // User Management
        [HttpGet("users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<object>();
            
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailConfirmed = user.EmailConfirmed,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    Roles = roles
                });
            }
            
            return Ok(userList);
        }

        [HttpGet("users/students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var studentList = students.Select(s => new
            {
                Id = s.Id,
                Email = s.Email,
                FirstName = s.FirstName,
                LastName = s.LastName,
                CreatedAt = s.CreatedAt
            });
            
            return Ok(studentList);
        }

        [HttpGet("users/professors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProfessors()
        {
            var professors = await _userManager.GetUsersInRoleAsync("Professor");
            var professorList = professors.Select(p => new
            {
                Id = p.Id,
                Email = p.Email,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Phone = p.Phone,
                Availability = p.Availability,
                CreatedAt = p.CreatedAt
            });
            
            return Ok(professorList);
        }

        [HttpPost("users/student")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDTO student)
        {
            // Only admins can create students through this endpoint
            // Public registration creates students automatically
            if (student == null)
            {
                return BadRequest("Student data is required");
            }

            // Create User
            var user = new User
            {
                UserName = student.Email,
                Email = student.Email,
                FirstName = student.FirstName,
                LastName = student.LastName,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, student.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Add to Student role
            if (!await _roleManager.RoleExistsAsync("Student"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Student"));
            }
            await _userManager.AddToRoleAsync(user, "Student");

            return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, new { user.Id, user.Email, Roles = new[] { "Student" } });
        }

        [HttpPost("users/professor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProfessor([FromBody] CreateProfessorDTO professor)
        {
            // Only admins can create professors - this endpoint is protected by [Authorize(Roles = "Admin")]
            if (professor == null)
            {
                return BadRequest("Professor data is required");
            }

            // Create User
            var user = new User
            {
                UserName = professor.Email,
                Email = professor.Email,
                FirstName = professor.FirstName,
                LastName = professor.LastName,
                Phone = professor.Phone,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, professor.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Add to Professor role
            if (!await _roleManager.RoleExistsAsync("Professor"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Professor"));
            }
            await _userManager.AddToRoleAsync(user, "Professor");

            return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, new { user.Id, user.Email, Roles = new[] { "Professor" } });
        }

        [HttpPost("users/admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDTO admin)
        {
            // Only admins can create other admins - this endpoint is protected by [Authorize(Roles = "Admin")]
            if (admin == null)
            {
                return BadRequest("Admin data is required");
            }

            // Create User
            var user = new User
            {
                UserName = admin.Email,
                Email = admin.Email,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, admin.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Add to Admin role
            await _userManager.AddToRoleAsync(user, "Admin");

            return CreatedAtAction(nameof(GetAllUsers), new { id = user.Id }, new { user.Id, user.Email, Roles = new[] { "Admin" } });
        }

        [HttpPut("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserDTO userUpdate)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Update basic fields
            if (!string.IsNullOrEmpty(userUpdate.FirstName))
                user.FirstName = userUpdate.FirstName;
            if (!string.IsNullOrEmpty(userUpdate.LastName))
                user.LastName = userUpdate.LastName;
            if (!string.IsNullOrEmpty(userUpdate.Email))
            {
                user.Email = userUpdate.Email;
                user.UserName = userUpdate.Email;
            }
            if (!string.IsNullOrEmpty(userUpdate.Phone))
                user.Phone = userUpdate.Phone;
            
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Update password if provided
            if (!string.IsNullOrEmpty(userUpdate.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, userUpdate.Password);
            }

            return Ok(new { user.Id, user.Email, user.FirstName, user.LastName });
        }

        [HttpDelete("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        // Course Management
        [HttpGet("courses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpPost("courses")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCourse([FromBody] Courses course)
        {
            if (course == null)
            {
                return BadRequest("Course data is required");
            }

            try
            {
                var createdCourse = await _courseService.CreateCourseAsync(course);
                return CreatedAtAction(nameof(GetAllCourses), new { id = createdCourse.CId }, createdCourse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("courses/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            await _courseService.DeleteCourseAsync(id);
            return NoContent();
        }

        // Enrollment Management
        [HttpGet("enrollments")]
        [ProducesResponseType(typeof(IEnumerable<Enrollments>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEnrollments()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            return Ok(enrollments);
        }

        [HttpPost("enrollments")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEnrollment([FromBody] CreateEnrollmentDTO enrollment)
        {
            if (enrollment == null)
            {
                return BadRequest("Enrollment data is required");
            }

            var result = await _enrollmentService.EnrollStudentAsync(enrollment.StudentId, enrollment.CourseId);
            if (!result)
            {
                return BadRequest("Failed to create enrollment");
            }

            return CreatedAtAction(nameof(GetAllEnrollments), null, enrollment);
        }

        [HttpDelete("enrollments/{studentId}/{courseId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEnrollment(string studentId, int courseId)
        {
            var result = await _enrollmentService.UnenrollStudentAsync(studentId, courseId);
            if (!result)
            {
                return NotFound("Enrollment not found");
            }

            return NoContent();
        }

        // Schedule Management
        [HttpGet("schedules")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllSchedules()
        {
            var schedules = await _scheduleService.GetAllSchedulesAsync();
            return Ok(schedules);
        }

        [HttpGet("rooms")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRooms();
            return Ok(rooms);
        }

        [HttpPost("rooms")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomDTO room)
        {
            if (room == null)
            {
                return BadRequest("Room data is required");
            }

            if (string.IsNullOrWhiteSpace(room.RName))
            {
                return BadRequest("Room name is required");
            }

            if (room.RCapacity <= 0)
            {
                return BadRequest("Room capacity must be greater than 0");
            }

            if (string.IsNullOrWhiteSpace(room.RType))
            {
                return BadRequest("Room type is required");
            }

            try
            {
                var roomModel = new Rooms
                {
                    RName = room.RName,
                    RCapacity = room.RCapacity,
                    RType = room.RType,
                    RDescription = room.RDescription ?? string.Empty
                };

                var createdRoom = await _roomService.CreateRoomAsync(roomModel);
                return CreatedAtAction(nameof(GetAllRooms), new { id = createdRoom.RId }, createdRoom);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create room: {ex.Message}");
            }
        }

        [HttpPost("schedules")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSchedule([FromBody] PostScheduleDTO schedule)
        {
            if (schedule == null)
            {
                return BadRequest("Schedule data is required");
            }

            // TODO: Implement schedule creation logic
            // var createdSchedule = await _scheduleService.CreateScheduleAsync(schedule);
            return CreatedAtAction(nameof(GetAllSchedules), null, schedule);
        }

        [HttpPut("schedules/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateSchedule(int id, [FromBody] PostScheduleDTO schedule)
        {
            if (schedule == null)
            {
                return BadRequest("Schedule data is required");
            }

            // TODO: Implement schedule update logic
            // var updatedSchedule = await _scheduleService.UpdateScheduleAsync(id, schedule);
            // if (updatedSchedule == null) return NotFound("Schedule not found");
            return Ok(schedule);
        }

        [HttpDelete("schedules/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            // TODO: Implement schedule deletion logic
            // var result = await _scheduleService.DeleteScheduleAsync(id);
            // if (!result) return NotFound("Schedule not found");
            return NoContent();
        }
    }

    public class CreateRoomDTO
    {
        public string RName { get; set; } = string.Empty;
        public int RCapacity { get; set; }
        public string RType { get; set; } = string.Empty;
        public string? RDescription { get; set; }
    }
}
