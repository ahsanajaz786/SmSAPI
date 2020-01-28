using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pathoschild.Http.Client.Internal;
using WebApiJwt.Entities;
using WebApiJwt.Models;
using WebApiJwt.ViewModels;

namespace WebApiJwt.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TodosController : ControllerBase

    {
        private readonly IHostingEnvironment hostingEnvironment;
        Image image;
        public ApplicationDbContext _context { get; }
        string tenantClaim { get; set; }
        public TodosController(ApplicationDbContext _context, ClaimsPrincipal c, IHostingEnvironment hostingEnvironment)
        {
            this._context = _context;
            this.hostingEnvironment = hostingEnvironment;
            tenantClaim = c.Claims.Where(e => e.Type == "UserID").FirstOrDefault().Value;
        }

        [Route("DeteleTodos")]
        [HttpGet]
        public async Task<ActionResult> AddTodosAsync(int id)
        {
            var d = _context.todos.FirstOrDefault(o => o.id == id);
            _context.Entry(d).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            _context.SaveChanges();
            return Ok
                ();

        }
        [Route("AddTodos")]
        [HttpPost]
        public async Task<ActionResult> AddTodosAsync([FromForm] TodoDto todoDto)
            {
            
            if (todoDto.id==0)
            {

                CultureInfo culture = new CultureInfo("en-US");
                DateTime tempDate = Convert.ToDateTime(todoDto.date, culture);

                string fileName = "";
                var file_name = "";
                // image = Base64ToImage(todoDto.fileUpload);

                if (todoDto.fileUpload != null)
                {
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    if (todoDto.fileUpload.Length > 0)
                    {
                        fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(todoDto.fileUpload.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await todoDto.fileUpload.CopyToAsync(fileStream);
                            file_name = todoDto.fileUpload.FileName
                                ;
                        }

                    }
                }
               // String date = todoDto.date.ToLongDateString() + todoDto.time.ToShortTimeString();

                DateTime dateTime = DateTime.Now;
                Todo todo = new Todo
                {
                    
                    title = todoDto.title,
                    description = todoDto.description,
                    score = todoDto.score
                ,
                    date_Time = tempDate,
                    type = todoDto.type,
                    section_Id = todoDto.section_Id
                ,

                    subject_Id = todoDto.subject_Id,
                    class_Id = todoDto.class_Id
                    ,
                    teacher_Id = tenantClaim,
                    fileName = file_name,
                    fileUploadPath = "uploads/" + fileName


                };

                _context.Entry(todo).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                var data = _context.todos.FirstOrDefault(o=>o.id==todoDto.id);
                CultureInfo culture = new CultureInfo("en-US");
                DateTime tempDate = Convert.ToDateTime(todoDto.date, culture);

                data.class_Id = todoDto.class_Id;
                data.title = todoDto.title;
                data.description = todoDto.description;
                data.score = todoDto.score;
                data.section_Id = todoDto.section_Id;
                data.teacher_Id = tenantClaim;
                data.type = todoDto.type;
                data.date_Time = tempDate;
                if(todoDto.attach_file_flag==-1)
                {

                }else if(todoDto.attach_file_flag==0)
                {
                    var fileName = "";
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                    if (todoDto.fileUpload.Length > 0)
                    {
                        fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(todoDto.fileUpload.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await todoDto.fileUpload.CopyToAsync(fileStream);
                            data.fileName = todoDto.fileUpload.FileName;
                            data.fileUploadPath = fileName;
                        }

                    }
                }
                else if (todoDto.attach_file_flag==1)
                {
                    var fileName = "";
                    var deletefile = Path.Combine(hostingEnvironment.WebRootPath);
                    var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");

                    if (todoDto.fileUpload.Length > 0)
                    {
                        if (System.IO.File.Exists((Path.Combine(deletefile, data.fileUploadPath))))
                        {
                            
                            System.IO.File.Delete((Path.Combine(deletefile, data.fileUploadPath)));
                            
                        }


                        fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(todoDto.fileUpload.FileName);
                        using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                        {
                            await todoDto.fileUpload.CopyToAsync(fileStream);
                            data.fileName = todoDto.fileUpload.FileName;
                            data.fileUploadPath = "uploads/"+ fileName;
                        }

                    }
                }
                _context.Entry(data).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                return Ok();
            }
        }

        [HttpGet("GetTodoBySection")]
        public ActionResult GetTodoBySection(int id)
        {
            var data = (from k in _context.todos.Where(o => o.is_deleted != true)
                        join l in _context.subjects on k.subject_Id equals l.id
                        join c in _context.classes on k.class_Id equals c.id
                        join u in _context.Users on k.teacher_Id equals u.Id

                        join s in _context.class_Sections on k.section_Id equals s.id
                        join section in _context.section on s.section_id equals section.id
                        select new
                        {

                            section=k.section_Id,
                            k.id,
                            k.is_deleted,
                            k.title,
                            type = Enum.GetName(typeof(Todo_Type.type), k.type),
                            k.description,
                            time = string.Format("{0:hh:mm:ss tt}", k.date_Time.ToShortTimeString()),
                            date = k.date_Time.ToString("ddd, dd MMM yyy")
                           ,
                            subject = l.subject_Name,
                            c.level
                            ,teacherID=u.Id,

                            u.FullName,

                            k.score,


                            section_Name = section.sectionName
                        }).Where(o=>o.section==id&& o.teacherID==tenantClaim).ToList();

            return new JsonResult(new
            {
                todoData = data

            });
        }
        [Route("GetTodos")]
        [HttpGet]
        public ActionResult GetTodos(int? id = 0)

        {
            if (id == 0)
            {
                var data = (from k in _context.todos.Where(o=>o.is_deleted!=true)
                            join l in _context.subjects on k.subject_Id equals l.id
                            join c in _context.classes on k.class_Id equals c.id
                            join u in _context.Users on k.teacher_Id equals u.Id
                            
                            join s in _context.class_Sections on k.section_Id equals s.id
                            join section in _context.section on s.section_id equals section.id
                            select new
                            {
                                k.id,
                                teacherid = k.teacher_Id,

                                k.is_deleted,
                                k.title,
                                type = Enum.GetName(typeof(Todo_Type.type), k.type),
                                k.description,
                                time = string.Format("{0:hh:mm:ss tt}", k.date_Time.ToShortTimeString()),
                                date = k.date_Time.ToString("ddd, dd MMM yyy")
                               ,
                                subject = l.subject_Name,
                                c.level,
                                u.FullName,

                                k.score,


                                section_Name=    section.sectionName
                            }).Where(o=>o.teacherid==tenantClaim).ToList();
                
                return new JsonResult(new
                {
                    todoData = data

                });
            }
            else
            {

                var data = (from k in _context.todos.Where(o => o.is_deleted != true)
                            join l in _context.subjects on k.subject_Id equals l.id
                            join c in _context.classes on k.class_Id equals c.id
                            join u in _context.Users on k.teacher_Id equals u.Id
                            join s in _context.class_Sections on k.section_Id equals s.id
                            join section in _context.section on s.section_id equals section.id

                            select new
                            {
                                teacherid=k.teacher_Id,
                                k.id,
                                sectionId = k.section_Id,
                                k.title,
                                type = Enum.GetName(typeof(Todo_Type.type), k.type),
                                k.description,
                                time = string.Format("{0:hh:mm:ss tt}", k.date_Time.ToShortTimeString()),
                                date = k.date_Time.ToString("ddd, dd MMM yyy"),
                                subject = l.subject_Name,
                                subjectid = l.id,
                                c.level,
                                u.FullName,

                                k.score,


                              section_Name=  section.sectionName
                            }).Where(o => o.subjectid == id && o.teacherid==tenantClaim).ToList();

                return new JsonResult(data);
            }

        }
        [Route("GetTodosBySubject")]
        [HttpGet]
        public ActionResult GetTodosBySubject(int id ,int sectionid )

        {
            if (id == 0)
            {
                var data = (from k in _context.todos.Where(o => o.is_deleted != true)
                            join l in _context.subjects on k.subject_Id equals l.id
                            join c in _context.classes on k.class_Id equals c.id
                            join u in _context.Users on k.teacher_Id equals u.Id
                            join s in _context.class_Sections on k.section_Id equals s.id
                            join section in _context.section on s.section_id equals section.id

                            select new
                            {
                                k.id,
                                k.title,
                                section=k.section_Id,
                                type = Enum.GetName(typeof(Todo_Type.type), k.type),
                                k.description,
                                time = string.Format("{0:hh:mm:ss tt}", k.date_Time.ToShortTimeString()),
                                date = k.date_Time.ToString("ddd, dd MMM yyy")
                               ,
                                subject = l.subject_Name,
                                subject_id=l.id,
                                c.level,
                                u.FullName,
                                teacherId=u.Id,

                                k.score,


                               section_Name= section.sectionName
                            }).Where(o=>o.subject_id==id&&o.section==sectionid &&o.teacherId==tenantClaim ).ToList();

                return new JsonResult(new
                {
                    todoData = data

                });
            }
            else
            {

                var data = (from k in _context.todos.Where(o => o.is_deleted != true)
                            join l in _context.subjects on k.subject_Id equals l.id
                            join c in _context.classes on k.class_Id equals c.id
                            join u in _context.Users on k.teacher_Id equals u.Id
                            join s in _context.class_Sections on k.section_Id equals s.id
                            join section in _context.section on s.section_id equals section.id

                            select new
                            {
                                k.id,
                                sectionId = k.section_Id,
                                k.title,
                                type = Enum.GetName(typeof(Todo_Type.type), k.type),
                                k.description,
                                time = string.Format("{0:hh:mm:ss tt}", k.date_Time.ToShortTimeString()),
                                date = k.date_Time.ToString("ddd, dd MMM yyy"),
                                subject = l.subject_Name,
                                subjectid = l.id,
                                c.level,
                                u.FullName,
                                teacherid = u.Id,

                                k.score,


                                section_Name = section.sectionName
                            }).Where(o => o.subjectid == id && o.sectionId== sectionid && o.teacherid==tenantClaim).ToList();

                return new JsonResult(data);
            }

        }

       
       
        [HttpGet("GetStudentsByClassSection")]
        public JsonResult GetSchoolSection(int id)
        {
            var data = (from k in _context.student_Classes.Where(o => o.is_deleted != true)
                        join l in _context.Users on k.student_Id equals l.Id
                        select new
                        {
                            l.Id,
                            k.class_Section_Id,
                            l.FullName,
                        }
                      ).Where(o => o.class_Section_Id == id).ToList();
            return new JsonResult(data);
        }


        [Route("AddSbjectScore")]
        [HttpPost]
        public ActionResult AddSbjectScore([FromBody] SubjectScore[] subject_score)
        {
            if (ModelState.IsValid)
            {
                foreach (var subject in subject_score)
                {
                    SubjectScore subject_ = new SubjectScore()
                    {
                        subject_Id = subject.subject_Id,
                        student_Id = subject.student_Id,
                        teacher_Id = tenantClaim,
                        score = subject.score,
                        todoId = subject.todoId,
                        status = false

                    };

                    _context.Entry(subject_).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    _context.SaveChanges();
                }
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }


        [Route("GetSubjectScore")]
        [HttpGet]
        public IActionResult GetSubjectScore(int? subject_id=0, int? section_id=0)
        {

            if (subject_id == 0 && section_id == 0)
            {
                var query = (from s in _context.todos
                             where s.teacher_Id == tenantClaim && s.is_deleted != true
                             join t in _context.Users on s.teacher_Id equals t.Id
                             join sub in _context.subjects on s.subject_Id equals sub.id
                             join tecsub in _context.teacher_Subjects on s.subject_Id equals tecsub.subject_Id
                             join sec in _context.class_Sections on s.section_Id equals sec.id
                             join cls in _context.classes on s.class_Id equals cls.id
                             join section in _context.section on sec.section_id equals section.id

                             where s.section_Id == tecsub.class_section
                             //join section in _context.class_Sections on t.section_Id equals section.id
                             select new
                             {
                                 todo_id = s.id
                                 ,

                                 section_Name = section.sectionName,
                                 cls.level

                                 //  section.section_Name
                                 ,
                                 sec = s.section_Id
                                 ,
                                 subject_id = s.subject_Id,

                                 todo_title = s.title,
                                 subject_name = sub.subject_Name,
                                 todo_type = Enum.GetName(typeof(Todo_Type.type), s.type),

                                 todo_score = s.score,
                                 teacher_name = t.FullName,
                                 profile_picture = "/assets/defaultimg.jpg",
                                 todo_date = s.date_Time.ToString("ddd, dd MMM yyy"),
                                 passed_students = _context.subjectScores.Where(o => o.todoId == s.id && ((o.score / s.score) * 100) >= 50).Count()

                                 ,
                                 total_students = _context.subjectScores.Where(o=>o.todoId==s.id).Count(),
                                 

                             }
                           ).ToList();

                return new JsonResult(query);
            }
            
            else if (subject_id != 0)
            {


                var query = (from s in _context.todos
                             where s.teacher_Id == tenantClaim && s.is_deleted != true
                             && s.section_Id == section_id && s.subject_Id == subject_id

                             join t in _context.Users on s.teacher_Id equals t.Id
                             where t.Id==tenantClaim
                             join sub in _context.subjects on s.subject_Id equals sub.id
                             join tecsub in _context.teacher_Subjects on s.subject_Id equals tecsub.subject_Id
                             join sec in _context.class_Sections on s.section_Id equals sec.id
                             join cls in _context.classes on s.class_Id equals cls.id
                             join section in _context.section on sec.section_id equals section.id

                             where s.section_Id == tecsub.class_section
                             //join section in _context.class_Sections on t.section_Id equals section.id
                             select new
                             {
                                 todo_id = s.id
                                 ,

                                 section_Name = section.sectionName,
                                 cls.level

                                 //  section.section_Name
                                 ,
                                 sec = s.section_Id
                                 ,
                                 subject_id = s.subject_Id,

                                 todo_title = s.title,
                                 subject_name = sub.subject_Name,
                                 todo_type = Enum.GetName(typeof(Todo_Type.type), s.type),

                                 todo_score = s.score,
                                 teacher_name = t.FullName,
                                 profile_picture = "/assets/defaultimg.jpg",
                                 todo_date = s.date_Time.ToString("ddd, dd MMM yyy"),
                                 /*    passed_students = (
                                        from studentcl in _context.student_Classes
                                        where studentcl.class_Section_Id == section_id
                                        join todoscr in _context.subjectScores on studentcl.student_Id equals todoscr.student_Id
                                        where todoscr.todoId == s.id && ((todoscr.score / s.score) * 100) > 33
                                        select todoscr.id
                                     ).Count(),
                                     total_students = (from studentcl in _context.student_Classes
                                                       where studentcl.class_Section_Id == section_id
                                                      select studentcl.id).Count()*/
                                 passed_students = _context.subjectScores.Where(o => o.todoId == s.id && ((o.score / s.score) * 100) >= 50).Count()

                                 ,
                                 total_students = _context.subjectScores.Where(o => o.todoId == s.id).Count(),

                             }
                           ).Where(o=>o.subject_id==subject_id).ToList();


                return new JsonResult(query);

            }
            else
        if ( section_id != 0)
            {

                var query = (from s in _context.todos
                             where s.teacher_Id == tenantClaim && s.is_deleted != true
                            && s.section_Id == section_id 
                             join t in _context.Users on s.teacher_Id equals t.Id
                             where t.Id==tenantClaim && s.is_deleted != true
                             join sub in _context.subjects on s.subject_Id equals sub.id
                             join tecsub in _context.teacher_Subjects on s.subject_Id equals tecsub.subject_Id
                             join sec in _context.class_Sections on s.section_Id equals sec.id
                             join cls in _context.classes on s.class_Id equals cls.id
                             join section in _context.section on sec.section_id equals section.id

                             where s.section_Id == tecsub.class_section
                             //join section in _context.class_Sections on t.section_Id equals section.id
                             select new
                             {
                                 todo_id = s.id
                                 ,

                                 section_Name = section.sectionName,
                                 cls.level

                                 //  section.section_Name
                                 ,
                                 sec = s.section_Id
                                 ,
                                 subject_id = s.subject_Id,

                                 todo_title = s.title,
                                 subject_name = sub.subject_Name,
                                 todo_type = Enum.GetName(typeof(Todo_Type.type), s.type),

                                 todo_score = s.score,
                                 teacher_name = t.FullName,
                                 profile_picture = "/assets/defaultimg.jpg",
                                 todo_date = s.date_Time.ToString("ddd, dd MMM yyy"),
                                 /*passed_students = (
                                    from studentcl in _context.student_Classes
                                    where studentcl.class_Section_Id == section_id
                                    join todoscr in _context.subjectScores on studentcl.student_Id equals todoscr.student_Id
                                    where todoscr.todoId == s.id && ((todoscr.score / s.score) * 100) > 33
                                    select todoscr.id
                                 ).Count(),
                                 total_students = (from studentcl in _context.student_Classes
                                                   where studentcl.class_Section_Id == section_id
                                                   select studentcl.id).Count()*/
                                 passed_students = _context.subjectScores.Where(o => o.todoId == s.id && ((o.score / s.score) * 100) >= 50).Count()

                                 ,
                                 total_students = _context.subjectScores.Where(o => o.todoId == s.id).Count(),


                             }
                           ).ToList();


                return new JsonResult(query);
            }
            else
            {
                var query = (from s in _context.todos
                             where s.teacher_Id == tenantClaim && s.is_deleted != true
                              && s.section_Id == section_id 
                             join t in _context.Users on s.teacher_Id equals t.Id
                             where t.Id==tenantClaim && s.is_deleted != true
                             join sub in _context.subjects on s.subject_Id equals sub.id
                             join tecsub in _context.teacher_Subjects on s.teacher_Id equals tecsub.teacher_Id
                             join sec in _context.class_Sections on s.section_Id equals sec.id
                             join cls in _context.classes on s.class_Id equals cls.id
                             join section in _context.section on sec.section_id equals section.id

                             where sec.id == s.section_Id && s.class_Id == cls.id && s.subject_Id == tecsub.subject_Id
                             //join section in _context.class_Sections on t.section_Id equals section.id
                             select new
                             {
                                 todo_id = s.id,
                                 sectionID = s.section_Id
                                 //  section.section_Name
                                 ,
                                 subject_id = s.subject_Id,

                               section_Name=  section.sectionName,
                                 cls.level
                                 ,
                                 todo_title = s.title,
                                 subject_name = sub.subject_Name,
                                 todo_type = Enum.GetName(typeof(Todo_Type.type), s.type),
                                 todo_score = s.score,
                                 teacher_name = t.FullName,
                                 profile_picture = "/assets/defaultimg.jpg",
                                 todo_date = s.date_Time.ToString("ddd, dd MMM yyy"),
                                 /*    passed_students = (
                                        from studentcl in _context.student_Classes
                                        where studentcl.class_Section_Id == section_id
                                        join todoscr in _context.subjectScores on studentcl.student_Id equals todoscr.student_Id
                                        where todoscr.todoId == s.id && ((todoscr.score / s.score) * 100) > 33
                                        select todoscr.id
                                     ).Count(),
                                     total_students = (from studentcl in _context.student_Classes
                                                       where studentcl.class_Section_Id == section_id
                                                       select studentcl.id).Count()*/
                                 passed_students = _context.subjectScores.Where(o => o.todoId == s.id && ((o.score / s.score) * 100) >= 50).Count()

                                 ,
                                 total_students = _context.subjectScores.Where(o => o.todoId == s.id).Count(),

                             }
                             ).Where(o => o.sectionID == section_id).ToList();

                return new JsonResult(query);

            }
            }
        
        [HttpGet("GetScoreByStudentAndSubject")]
        public IActionResult GetScoreByStudentAndSubject(string studentID,int? subjectID=0)
        {



            if ( subjectID == 0)
            {
                var query = (from s in _context.subjectScores
                             join t in _context.todos on s.todoId equals t.id
                             //  where s.class_section_Id == section_id
                             join sub in _context.subjects on s.subject_Id equals sub.id
                             join cls in _context.classes on t.class_Id equals cls.id
                             //? t.subject_Id : 1 equals subject_id.HasValue ? subject.id : 1
                             join teacher in _context.Users on t.teacher_Id equals teacher.Id
                             where teacher.Id ==tenantClaim && t.is_deleted != true
                             join sec in _context.class_Sections on t.section_Id equals sec.id
                             join section in _context.section on sec.section_id equals section.id

                             select new
                             {
                                 s.student_Id,
                                 todo_id = t.id,

                                section_Name= section.sectionName
                                 ,
                                 ///student_id=stu.student_Id,
                                 classname = cls.level,
                                 subject_id = t.subject_Id,
                                 todo_title = t.title,
                                 subject_name = sub.subject_Name,
                                 todo_type = Enum.GetName(typeof(Todo_Type.type), t.type),
                                 todo_score = t.score,
                                 teacher_name = teacher.FullName,
                                 profile_picture = "/assets/defaultimg.jpg",
                                 todo_date = t.date_Time.ToString("ddd, dd MMM yyy"),

                             }).Where(o=>o.student_Id==studentID).ToList();
                             return new JsonResult(query);
            }
            else
            {
                var query = (from s in _context.subjectScores
                             join t in _context.todos on s.todoId equals t.id
                             //  where s.class_section_Id == section_id
                             join sub in _context.subjects on s.subject_Id equals sub.id
                             join cls in _context.classes on t.class_Id equals cls.id
                             //? t.subject_Id : 1 equals subject_id.HasValue ? subject.id : 1
                             join teacher in _context.Users on t.teacher_Id equals teacher.Id
                             where teacher.Id==tenantClaim && t.is_deleted != true
                             join sec in _context.class_Sections on t.section_Id equals sec.id
                             join section in _context.section on sec.section_id equals section.id

                             select new
                             {
                                 s.student_Id,
                                 todo_id = t.id,

                                 section_Name = section.sectionName
                                 ,
                                 ///student_id=stu.student_Id,
                                 classname = cls.level,
                                 subject_id = t.subject_Id,
                                 todo_title = t.title,
                                 subject_name = sub.subject_Name,
                                 todo_type = Enum.GetName(typeof(Todo_Type.type), t.type),
                                 todo_score = t.score,
                                 teacher_name = teacher.FullName,
                                 profile_picture = "/assets/defaultimg.jpg",
                                 todo_date = t.date_Time.ToString("ddd, dd MMM yyy"),

                             }).Where(o=>o.student_Id==studentID && o.subject_id==subjectID).ToList();
                return new JsonResult(query);

            }
           
        }

       

        [HttpGet("GetTodoScoreBySubjectID")]
        public IActionResult GettodoBySubjectID(int id,int sectionID)
        {
            var query = (from s in _context.class_Subjects

                         where s.class_section_Id == id
                         join t in _context.todos on s.subject_Id equals t.subject_Id
                         join teacher in _context.Users on t.created_by equals teacher.Id
                         where teacher.Id == tenantClaim && t.is_deleted != true

                         join subject in _context.subjects on t.subject_Id equals subject.id // ?  : 1 equals subject_id.HasValue ? subject.id : 1

                         select new
                         {
                             todo_id = t.id,
                             subject_id = t.subject_Id,
                             todo_title = t.title,
                             subject_name = subject.subject_Name,
                             todo_type = Enum.GetName(typeof(Todo_Type.type), t.type),
                             todo_score = t.score,
                             teacher_name = teacher.FullName,
                             profile_picture = "/assets/defaultimg.jpg",
                             todo_date = t.date_Time.ToString("ddd, dd MMM yyy"),
                             passed_students = (
                                from studentcl in _context.student_Classes
                                where studentcl.class_Section_Id == sectionID
                                join todoscr in _context.subjectScores on studentcl.student_Id equals todoscr.student_Id
                                where todoscr.todoId == t.id && ((todoscr.score / t.score) * 100) > 33
                                && todoscr.subject_Id == id
                                select todoscr.id
                             ).Count(),
                             total_students = (from studentcl in _context.student_Classes
                                               where studentcl.class_Section_Id == sectionID

                                               select studentcl.id).Count()
                         }
                         ).Where(o => o.subject_id == id).ToList();
            return new JsonResult(query);

        }

        [Route("GetTodoScoreByStudentIDandSubject")]
        [HttpGet]
      /*  public IActionResult GetTodoScoreByStudentIDandSubject(int student_Id , int subjetct_Id,int )
        {

            
                var query = (from s in _context.class_Subjects
                                 //  where s.class_section_Id == section_id
                             join t in _context.todos on s.subject_Id equals t.subject_Id
                             join teacher in _context.Users on t.created_by equals teacher.Id
                             join subject in _context.subjects on  t.subject_Id equals subject.id
                             select new
                             {
                                 todo_id = t.id,
                                 student_Id=s.id,

                                 subject_id = t.subject_Id,
                                 todo_title = t.title,
                                 subject_name = subject.subject_Name,
                                 todo_type = t.type,
                                 todo_score = t.score,
                                 teacher_name = teacher.FullName,
                                 profile_picture = "/assets/defaultimg.jpg",
                                 todo_date = t.date_Time.ToString("ddd, dd MMM yyy"),
                                 passed_students = (
                                    from studentcl in _context.student_Classes
                                    where studentcl.class_Section_Id == section_id
                                    join todoscr in _context.subjectScores on studentcl.student_Id equals todoscr.student_Id
                                    where todoscr.todoId == t.id && ((todoscr.score / t.score) * 100) > 33
                                    select todoscr.id
                                 ).Count(),
                                 total_students = (from studentcl in _context.student_Classes
                                                   where studentcl.class_Section_Id == section_id
                                                   select studentcl.id).Count()
                             }
                           ).Where(o=>o.student_Id==student_Id && o).ToList();
                return new JsonResult(query);
            
        }
        */

        [Route("GetTodoScore")]
        [HttpGet]
        public IActionResult GetTodoScore(int todo_id)
        {
            var todo = (from t in _context.todos
                        where t.teacher_Id == tenantClaim && t.is_deleted != true

                        where t.id == todo_id
                        join teach in _context.Users on t.created_by equals teach.Id
                        select new
                        {
                            todo_title = t.title,
                            teacher_name = teach.FullName,
                            teacher_picture = "/assets/defaultimg.jpg",
                            todo_score = t.score,
                            section_id = t.section_Id,
                            students = (
                                            from s in _context.student_Classes
                                            where s.class_Section_Id == t.section_Id
                                            join stu in _context.Users on s.student_Id equals stu.Id
                                            join stuscr in _context.subjectScores on new { stu = stu.Id, todo=t.id} equals new { stu = stuscr.student_Id, todo = stuscr.todoId}
                                             into TScr
                                            from FScr in TScr.DefaultIfEmpty()
                                            select new
                                            {
                                                id = FScr.id != null ? FScr.id : 0,
                                                name = stu.FullName,
                                                student_id = stu.Id,
                                                subject_Id=t.subject_Id,
                                                todoId = todo_id,
                                               score = FScr.score != null ? FScr.score.ToString() : "0.0"
                                            }
                                        ).ToList()
                        }).FirstOrDefault();

            return new JsonResult(todo);
        }

        [HttpPost("SaveTodoScore")]
        public IActionResult SaveTodoScore([FromRoute] int todoId, [FromBody] List<Save_Todo_Score> scores, [FromServices] TenantInformation ti)
        {
            scores.ForEach(e =>
            {
                SubjectScore score = null;
                if (e.id > 0)
                {
                    score = _context.subjectScores.Where(s => s.id == e.id).FirstOrDefault();
                    _context.Entry(score).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                else
                {
                    score = new SubjectScore();
                    score.student_Id = e.student_id;
                    score.todoId = todoId;
                    score.subject_Id = e.subject_id;
                    score.todoId = e.todoid;

                    score.status = false;
                    score.teacher_Id = ti.UserId;
                    score.year_id = ti.year_id;
                    _context.Entry(score).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                double sData = Double.Parse(e.score);
                score.score = sData;
            });
            _context.SaveChanges();
            return new JsonResult(new
            {
                message = "Done"
            });
        }



        [HttpGet("GetClassSection")]
        public ActionResult GetClassSection()
        {
            var data = (from k in _context.classes.Where(o => o.is_deleted != true)
                      
                        join l in _context.class_Sections on k.id equals l.class_ID
                        join t in _context.teacher_Subjects on l.id equals t.class_section
                        join s in _context.subjects on t.subject_Id equals s.id
                        join section in _context.section on l.section_id equals section.id

                        select new
                        {

                            id = l.id,
                            t.teacher_Id,

                            advisor = l.advisor == tenantClaim ? true : false,
                            classSection = k.level + " - " + section.sectionName,
                            subjectname=s.subject_Name,
                            subject_id=s.id,
                            classId=k.id
                        }).Where(o=>o.teacher_Id==tenantClaim).ToList();

            return new JsonResult(new
            {
                Class_Sections = data

            });
        }
        [HttpGet("EditTodo")]
        public ActionResult editTodo(int id)
        {
            var todoEdit = _context.todos.FirstOrDefault(o=>o.id==id);
            var classSection= (from k in _context.classes.Where(o => o.is_deleted != true)
                               join l in _context.class_Sections on k.id equals l.class_ID
                               join section in _context.section on l.section_id equals section.id

                               select new
                              {

                                  id = l.id,
                                 
                                  classSection = k.level + "- " + section.sectionName
                              }).ToList();
            return new JsonResult(new
            {
                editTodo = todoEdit,
                classSection=classSection

            });
        }


        [HttpGet("GetClass")]
        public ActionResult GetClass()
        {
            var data = (from k in _context.classes.Where(o=>o.is_deleted!=true)
                       
                        select new
                        {

                            id = k.id,
                            classSection = k.level 
                        }).ToList();

            return new JsonResult(new
            {
                Class_Sections = data

            });
        }
        [Route("GetTodosDetails")]
        [HttpGet]
        public ActionResult GetTodosDetails(int id)


        {
            var data = (from k in _context.todos
                        join l in _context.subjects on k.subject_Id equals l.id
                        join c in _context.classes on k.class_Id equals c.id
                        join u in _context.Users on k.teacher_Id equals u.Id
                        join s in _context.class_Sections on k.section_Id equals s.id
                        join section in _context.section on s.section_id equals section.id

                        select new
                        {
                            k.id,
                            k.title,
                            type = Enum.GetName(typeof(Todo_Type.type), k.type),
                            k.description,
                            time = string.Format("{0:hh:mm:ss tt}", k.date_Time.ToShortTimeString()),
                            date = k.date_Time.ToShortDateString()
                           ,
                            subject = l.subject_Name,
                            c.level,
                            u.FullName,
                            k.fileName,
                            filename = k.fileName,
                            filepath = k.fileUploadPath,
                            k.score,


                           section_Name= section.sectionName
                        }).FirstOrDefault(o => o.id == id);

            return new JsonResult(new
            {
                todoData = data

            });




        }

        

        [HttpGet("GetSectionSubject")]
        public ActionResult GetSectionSubject(int id)
        {
            var adviser = _context.class_Sections.Where(o => o.advisor == tenantClaim).Count();
            if (adviser > 0)
            {
                var tdata = (from k in _context.teacher_Subjects
                             join l in _context.class_Sections on k.class_section equals l.id

                             join m in _context.subjects on k.subject_Id equals m.id
                             select new
                             {
                                 adviser=l.advisor,
                                 k.teacher_Id,
                                 section= l.id,
                                 
                                 
                                 k.class_section,
                                 m.title,
                                 m.id,
                                 name=m.subject_Name
                             }
                             ).Where(o=>o.adviser==tenantClaim && o.class_section== id).ToList();
                var data = (from k in _context.teacher_Subjects
                              join  l in _context.class_Sections on  k.class_section equals l.id
                             join m in _context.subjects on k.subject_Id equals m.id
                            // join c in _context.class_Sections on k.section_Id equals c.id
                             select new
                             {
                                 l.advisor,
                                 k.teacher_Id
                                 ,k.class_section,
                                 section = l.id,
                                 m.title,
                                 m.id,
                                 name = m.subject_Name
                             }
                             ).Where(o=> o.class_section == id && o.advisor!=tenantClaim&& o.teacher_Id== tenantClaim).ToList();

                List<Get_Section_Subject> list = new List<Get_Section_Subject>();
               foreach(var v in tdata)
                {
                    Get_Section_Subject d = new Get_Section_Subject();
                    d.name = v.name;
                    d.section_ID = v.section;
                    d.name = v.name;
                    d.subject_id = v.id;
                    d.title = v.title;
                    d.teacher_ID = v.teacher_Id;
                    list.Add(d);


                }
               
                // list.Add(new Get_Section_Subject { name= "All Subject",s})
                foreach (var v in data)
                {
                    Get_Section_Subject d = new Get_Section_Subject();
                    d.name = v.name;
                    d.section_ID = v.section;
                    d.name = v.name;
                    d.subject_id = v.id;
                    d.title = v.title;
                    d.teacher_ID = v.teacher_Id;
                    list.Add(d);


                }
                Get_Section_Subject da = new Get_Section_Subject();
                da.name = "All Subjects";
                da.title = "All Subjects";
                list.Add(da);

                return new JsonResult(

                    new
                    {
                        SectionSubject = list

                    });
            }
            else
            {
                var data = (from k in _context.teacher_Subjects
                            join l in _context.subjects on k.subject_Id equals l.id
                            join s in _context.class_Sections on k.section_Id equals s.id
                            select new
                            {
                                teacher_id=k.teacher_Id,
                                section_id = s.id
                                ,k.class_section,
                                l.subject_Name,
                                l.title
                            }
                             ).Where(o => o.class_section == id && o.teacher_id== tenantClaim).ToList();

                return new JsonResult(

                    new
                    {
                        SectionSubject = data

                    });

            }

        }


        public class TodoDto
        {


            public int id { get; set; }

            //  [Required]
            public string title { get; set; }
            //  [Required]
            public String description { get; set; }
            //   [Required]
           public String date { get; set; }
            //   [Required]
            //public DateTime time { get; set; }
            //   [Required]
            public Todo_Type.type type { get; set; }
            //   [Required]
            public double score { get; set; }
            //   [Required]GetSubjectScore
            public int subject_Id { get; set; }

            //  [Required]
            public int class_Id { get; set; }
            //[Required]
            public int section_Id { get; set; }
            //[Required]
            public int year_Id { get; set; }
            //[Required]
            public int school_Id { get; set; }
            //[Required]
            public int attach_file_flag { get; set; }
            public IFormFile fileUpload { get; set; }

        }
        public class SubjectScoreDto
        {
            [Required]

            public int subject_Id { get; set; }
            [Required]

            public int teacher_Id { get; set; }
            [Required]

            public int student_Id { get; set; }
            [Required]

            public int score { get; set; }

        }
    }
}