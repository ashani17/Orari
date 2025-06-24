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
using Orari.DTO.EnrollmentDTO;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Orari.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Produces("application/json")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICourseService _courseService;
        private readonly IScheduleService _scheduleService;
        private readonly IRoomService _roomService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IEnrollmentService enrollmentService,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            ICourseService courseService,
            IScheduleService scheduleService,
            IRoomService roomService,
            ILogger<AdminController> logger)
        {
            _enrollmentService = enrollmentService;
            _userManager = userManager;
            _roleManager = roleManager;
            _courseService = courseService;
            _scheduleService = scheduleService;
            _roomService = roomService;
            _logger = logger;
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
                    id = user.Id,
                    email = user.Email,
                    userName = user.UserName,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    phone = user.Phone,
                    emailConfirmed = user.EmailConfirmed,
                    createdAt = user.CreatedAt,
                    updatedAt = user.UpdatedAt,
                    roles = roles.ToArray()
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

        [AllowAnonymous]
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

        [HttpGet("users/admins")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var adminList = admins.Select(a => new
            {
                Id = a.Id,
                Email = a.Email,
                FirstName = a.FirstName,
                LastName = a.LastName,
                CreatedAt = a.CreatedAt
            });
            
            return Ok(adminList);
        }

        [AllowAnonymous]
        [HttpGet("users/admins-public")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAdminsPublic()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var adminList = admins.Select(a => new
            {
                Id = a.Id,
                Email = a.Email,
                FirstName = a.FirstName,
                LastName = a.LastName,
                CreatedAt = a.CreatedAt
            });
            return Ok(adminList);
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
            _logger.LogInformation("CreateProfessor endpoint called");
            
            // Log authentication info
            _logger.LogInformation("User authenticated: {IsAuthenticated}", User.Identity?.IsAuthenticated);
            _logger.LogInformation("User name: {UserName}", User.Identity?.Name);
            
            // Log user claims
            var claims = User.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
            _logger.LogInformation("User claims: {Claims}", string.Join(", ", claims));
            
            // Log user roles
            var roles = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            _logger.LogInformation("User roles: {Roles}", string.Join(", ", roles));
            
            // Only admins can create professors - this endpoint is protected by [Authorize(Roles = "Admin")]
            if (professor == null)
            {
                _logger.LogWarning("CreateProfessor called with null professor data");
                return BadRequest("Professor data is required");
            }

            _logger.LogInformation("Creating professor with email: {Email}", professor.Email);

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
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create professor: {Errors}", errors);
                return BadRequest(result.Errors);
            }

            _logger.LogInformation("Professor user created successfully with ID: {UserId}", user.Id);

            // Add to Professor role
            if (!await _roleManager.RoleExistsAsync("Professor"))
            {
                _logger.LogInformation("Professor role doesn't exist, creating it");
                await _roleManager.CreateAsync(new IdentityRole("Professor"));
            }
            await _userManager.AddToRoleAsync(user, "Professor");

            _logger.LogInformation("Professor role assigned successfully");

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
        [AllowAnonymous]
        [HttpGet("courses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            
            // Map to DTOs to avoid circular references
            var courseDtos = courses.Select(c => new GetDelCourseDTO
            {
                CId = c.CId,
                CName = c.CName,
                Credits = c.Credits,
                PId = c.PId,
                Profesor = c.Profesor
            });
            
            return Ok(courseDtos);
        }

        [HttpGet("courses/with-enrollments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCoursesWithEnrollments()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            
            // Map to DTOs without accessing Enrollments navigation property to avoid circular references
            var courseDtos = new List<CourseWithEnrollmentsDTO>();
            
            foreach (var course in courses)
            {
                var courseDto = new CourseWithEnrollmentsDTO
                {
                    CId = course.CId,
                    CName = course.CName,
                    Credits = course.Credits,
                    PId = course.PId,
                    Profesor = course.Profesor,
                    // Don't access Enrollments navigation property to avoid circular reference
                    Enrollments = new List<Orari.DTO.EnrollmentDTO.EnrollmentSummaryDTO>()
                };
                
                courseDtos.Add(courseDto);
            }
            
            return Ok(courseDtos);
        }

        [HttpPost("courses")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCourse([FromBody] PostCourseDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Course data is required");
            }

            // No need to parse PId, just use as string
            try
            {
                var course = new Courses
                {
                    CName = dto.CName,
                    Credits = dto.Credits,
                    PId = dto.PId,
                    Profesor = dto.Profesor
                };
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

        [HttpPut("courses/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] PutCourseDTO putCourseDTO)
        {
            try
            {
                var existingCourse = await _courseService.GetCourseByIdAsync(id);
                if (existingCourse == null)
                {
                    return NotFound("Course not found");
                }

                // Update basic course properties
                if (!string.IsNullOrEmpty(putCourseDTO.CName))
                    existingCourse.CName = putCourseDTO.CName;
                if (putCourseDTO.Credits > 0)
                    existingCourse.Credits = putCourseDTO.Credits;
                if (!string.IsNullOrEmpty(putCourseDTO.ProfessorName))
                    existingCourse.Profesor = putCourseDTO.ProfessorName;

                // Update StudyProgramCourse relationship
                // Remove existing relationships
                existingCourse.StudyProgramCourse.Clear();
                
                // Add new relationship if StudyProgramId is greater than 0
                if (putCourseDTO.StudyProgramId > 0)
                {
                    existingCourse.StudyProgramCourse.Add(new Orari.Models.StudyProgramCourse
                    {
                        SPId = putCourseDTO.StudyProgramId,
                        CId = existingCourse.CId
                    });
                }

                var updatedCourse = await _courseService.UpdateCourseAsync(existingCourse);
                return Ok(updatedCourse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Enrollment Management
        [HttpGet("enrollments")]
        [ProducesResponseType(typeof(IEnumerable<EnrollmentWithDetailsDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEnrollments()
        {
            var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
            
            // Map to DTOs to avoid circular references
            var enrollmentDtos = enrollments.Select(e => new EnrollmentWithDetailsDTO
            {
                EId = e.EId,
                StudentId = e.StudentId,
                StudentName = $"{e.Student?.FirstName} {e.Student?.LastName}".Trim(),
                StudentEmail = e.Student?.Email ?? string.Empty,
                CId = e.CId,
                CourseName = e.Courses?.CName ?? string.Empty,
                CourseCredits = e.Courses?.Credits ?? 0,
                ProfessorName = e.Courses?.Profesor ?? string.Empty
            });
            
            return Ok(enrollmentDtos);
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
            
            // Map to DTOs to avoid circular references
            var scheduleDtos = schedules.Select(s => new GetDelScheduleDTO
            {
                SId = s.SId,
                Date = s.Date,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                RId = s.RId,
                ProfessorId = s.ProfessorId,
                CId = s.CId,
                EId = s.EId,
                RecurringScheduleId = s.RecurringScheduleId,
                RoomName = s.Room?.RName,
                CourseName = s.Course?.CName,
                ProfessorName = s.Professor != null ? $"{s.Professor.FirstName} {s.Professor.LastName}".Trim() : null
            });
            
            return Ok(scheduleDtos);
        }

        [AllowAnonymous]
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

            try
            {
                // Get the required entities
                var room = await _roomService.GetRoomByIdAsync(schedule.RId);
                var course = await _courseService.GetCourseByIdAsync(schedule.CId);
                
                if (room == null)
                {
                    return BadRequest("Room not found");
                }
                
                if (course == null)
                {
                    return BadRequest("Course not found");
                }

                // Map the PostScheduleDTO to the Schedules model
                var scheduleModel = new Schedules
                {
                    Date = schedule.Date,
                    StartTime = schedule.StartTime,
                    EndTime = schedule.EndTime,
                    RId = schedule.RId,
                    ProfessorId = schedule.ProfessorId,
                    CId = schedule.CId,
                    Room = room,
                    Course = course,
                    EId = null,  // No exam initially
                    Exam = null
                };

                var createdSchedule = await _scheduleService.CreateScheduleAsync(scheduleModel);
                return CreatedAtAction(nameof(GetAllSchedules), new { id = createdSchedule.SId }, createdSchedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            var result = await _scheduleService.DeleteScheduleAsync(id);
            if (!result) return NotFound("Schedule not found");
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
