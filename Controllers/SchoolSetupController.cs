using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApiJwt.Entities;
using WebApiJwt.Models;
using WebApiJwt.ViewModels;

namespace WebApiJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolSetupController : ControllerBase
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
       

        public ApplicationDbContext _context { get; }
        string tenantClaim { get; set; }
        int School_Claim { get; set; }
        int year_id { get; set; }
        public SchoolSetupController(ApplicationDbContext _context, ClaimsPrincipal c, IHostingEnvironment hostingEnvironment, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._context = _context;
            _userManager = userManager;
            _signInManager = signInManager;
            this.hostingEnvironment = hostingEnvironment;
            tenantClaim = c.Claims.Where(e => e.Type == "UserID").FirstOrDefault().Value;
            year_id =int.Parse( c.Claims.Where(e => e.Type == "year_id").FirstOrDefault().Value);
            School_Claim = int.Parse(c.Claims.Where(e => e.Type == "school_id").FirstOrDefault().Value);

        }
        [HttpPost("SaveClass")]
        public IActionResult saveClass([FromBody]Classes classe)

        {

            if (classe.id==0)
            {
                _context.classes.Add(classe);
                _context.SaveChanges();
            }
            else
            {
                var d = _context.classes.FirstOrDefault(o => o.id == classe.id);

                _context.Entry(d).State = Microsoft.EntityFrameworkCore.EntityState.Modified
                    ;
                d.level = classe.level;
                _context.SaveChanges();
            }
            return new JsonResult("Data Saved "); 
        }
        [HttpGet("getClass")]
        public IActionResult getClass()

        
        {
            var d = (from k in _context.classes
                     select new
                     {
                         k.id,
                         k.level
                     }).ToList();
                   
                ;
            return new JsonResult(d);

        }
        //Sections /*
        [HttpPost("SaveSection")]
        public IActionResult SaveSection([FromBody]Section section)

        {

            if (section.id == 0)
            {
                _context.section.Add(section);
                _context.SaveChanges();
            }
            else
            {
                var d = _context.section.FirstOrDefault(o => o.id == section.id);

                _context.Entry(d).State = Microsoft.EntityFrameworkCore.EntityState.Modified
                    ;
                d.sectionName = section.sectionName;
                _context.SaveChanges();
            }
            return new JsonResult("Data Saved ");
        }
        [HttpGet("GetSection")]
        public IActionResult GetSection()


        {
            var d = (from k in _context.section
                     select new
                     {
                         k.sectionName,
                         k.id
                     }).ToList();

            ;
            return new JsonResult(d);

        }
        //Subject
        [HttpPost("SaveSubject")]
        public IActionResult SaveSubject([FromBody]Subject subject)

        {

            if (subject.id == 0)
            {
                subject.subject_Name = subject.title;
                _context.subjects.Add(subject);
                _context.SaveChanges();
            }
            else
            {
                var d = _context.subjects.FirstOrDefault(o => o.id == subject.id);

                _context.Entry(d).State = Microsoft.EntityFrameworkCore.EntityState.Modified
                    ;
                d.subject_Name = subject.subject_Name;
                d.title = subject.title;
                _context.SaveChanges();
            }
            return new JsonResult("Data Saved ");
        }

        [HttpGet("GetSubject")]
        public IActionResult GetSubject()


        {
            var d = (from k in _context.subjects
                     select new
                     {
                         k.subject_Name,
                         k.title,
                         k.id
                     }).ToList();

            ;
            return new JsonResult(d);

        }
        
        //Teacher 
        [HttpPost("RegisterTeacher")]
        public async Task<IActionResult> RegisterTeacherAsync([FromForm]Models.TeacherDto teacher)

        {
            if (teacher.id == "0")
            {
                var user = new User
                {
                    UserName = teacher.UserName,
                    Email = teacher.Email,
                    FullName = teacher.FullName
                    ,
                    PhoneNumber = teacher.phone
                    ,
                    EmailConfirmed = true
                    ,
                    Timezone = TimeZone.CurrentTimeZone.StandardName.ToString()

                };
                var fileName = "";
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                if (teacher.profileimg.Length > 0)
                {
                    fileName = Guid.NewGuid().ToString().Replace("-", teacher.profileimg.FileName) + Path.GetExtension(teacher.profileimg.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        await teacher.profileimg.CopyToAsync(fileStream);
                        //emp.BookPic = fileName;
                    }

                }
                user.profilePath = "uploads/" + fileName;

                var result = await _userManager.CreateAsync(user, teacher.Password);
                if (result.Succeeded)
                {
                    Teacher t = new Teacher();
                    Schoo_User school_user = new Schoo_User();
                    school_user.school_Id= School_Claim; ;
                    school_user.user_Id = user.Id;
                    _context.schoo_Users.Add(school_user);
                    _context.SaveChanges();




                    t.school_id = School_Claim;
                    t.userid = user.Id;

                    // user.profilePath = "uploads/" + fileName;
                    _context.teachers.Add(t);
                    _context.SaveChanges();
                    return new JsonResult("Data Saved ");

                }
                else
                {
                    string error = "";
                    foreach (var e in result.Errors)
                    {
                        error += e.Description;
                    }
                    return new JsonResult(error);

                }
            }
            else
            {
                var v = _context.Users.FirstOrDefault(o => o.Id == teacher.id);
                v.FullName = teacher.FullName;
                v.Email = teacher.Email;
                v.PhoneNumber = teacher.phone;
                _context.Entry(v).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return new JsonResult("Data Saved ");
            }

           
        }
        [HttpGet("GetTeacher")]
        public IActionResult GetTeacher()


        {
            var d = (from k in _context.teachers    
                     join l in _context.Users on k.userid equals l.Id
                     select new
                     {
                         k.id,
                         phone=l.PhoneNumber,
                         user_id=l.Id,
                   //      l.profilePath,
                       name=  l.FullName,
                         l.Email
                     }).ToList();

            ;
            return new JsonResult(d);

        }
        //Class Section 
        [HttpPost("AddClassSection")]
        public async Task<IActionResult> ClassSection([FromBody]Class_Section class_Section)

        {
          if(class_Section.id==0)
            {
                _context.class_Sections.Add(class_Section);
                _context.SaveChanges();
            }
            else
            {
                var d = _context.class_Sections.FirstOrDefault(o => o.id == class_Section.id);
                d.class_ID = class_Section.class_ID;
                d.section_id = class_Section.section_id;
                d.advisor = class_Section.advisor;
                _context.Entry(d).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

            }


            return new JsonResult("Data Saved ");
        }
        [HttpGet("GetclassSection")]
        public IActionResult GetclassSection(int id)


        {
            var d = (from k in _context.class_Sections
                     join l in _context.Users on k.advisor equals l.Id
                     join c in _context.classes on k.class_ID equals c.id
                     join Section in _context.section on k.section_id equals Section.id


                     select new
                     {
                         k.id,
                         clasid=c.id,
                         l.FullName,
                         sectionID=k.section_id,
                         advisor=l.FullName,
                         Section.sectionName,
                         c.level
                     }).Where(o=>o.clasid==id).ToList();

            ;
            return new JsonResult(d);

        }

        //class Subject

        [HttpPost("AddClassSectionSubject")]
        public async Task<IActionResult> AddClassSectionSubject([FromBody]Class_Subject subject)

        {
            if (subject.id == 0)
            {
                _context.class_Subjects.Add(subject);
                _context.SaveChanges();
            }
            else
            {
                var d = _context.class_Subjects.FirstOrDefault(o => o.id == subject.id);
                d.subject_Id = subject.subject_Id;
                d.class_section_Id = subject.class_section_Id; ;
              
                _context.Entry(d).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

            }


            return new JsonResult("Data Saved ");
        }
        [HttpGet("GetClassSectionSubject")]
        public IActionResult GetClassSectionSubject(int? id=0)


        {
            if (id == 0)
            {
                var d = (from ss in _context.class_Subjects
                         join k in _context.class_Sections on ss.class_section_Id equals k.id
                         join s in _context.subjects on ss.subject_Id equals s.id
                         join c in _context.classes on k.class_ID equals c.id
                         join Section in _context.section on k.section_id equals Section.id


                         select new
                         {
                             classid = c.id,
                            
                             sectionId = Section.id,
                             subjectid = ss.subject_Id,
                             ss.id,
                             s.title,
                             s.subject_Name,
                             c.level,
                             Section.sectionName
                         }).ToList();

                ;
                return new JsonResult(d);
            }
            else
            {
                var d = (from ss in _context.class_Subjects
                         join k in _context.class_Sections on ss.class_section_Id equals k.id
                         join s in _context.subjects on ss.subject_Id equals s.id
                         join c in _context.classes on k.class_ID equals c.id
                         join Section in _context.section on k.section_id equals Section.id


                         select new
                         {
                             section_class_id=ss.class_section_Id,
                             classid = c.id,
                             classsecid=k.id,
                             sectionId = Section.id,
                             subjectid = ss.subject_Id,
                             ss.id,
                             s.title,
                             s.subject_Name,
                             c.level,
                             Section.sectionName
                         }).Where(o=>o.section_class_id == id).ToList();

                ;
                return new JsonResult(d);

            }
        }

        //Assign Teacher Subject

        [HttpPost("AssignTeacherSubject")]
        public async Task<IActionResult> AssignTeacherSubject([FromBody]Teacher_Subject teacher_Subject)

        {
            if (teacher_Subject.id == 0)
            {
                _context.teacher_Subjects.Add(teacher_Subject);
                _context.SaveChanges();
            }
            else
            {
                var d = _context.teacher_Subjects.FirstOrDefault(o => o.id == teacher_Subject.id);
                d.teacher_Id = teacher_Subject.teacher_Id;
                d.class_Id = teacher_Subject.class_Id;
                d.section_Id = teacher_Subject.section_Id;
                d.subject_Id = teacher_Subject.subject_Id;
                d.class_section = teacher_Subject.class_section;




                _context.Entry(d).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

            }


            return new JsonResult("Data Saved ");
        }
        [HttpGet("GetTeacherSubject")]
        public IActionResult GetTeacherSubject()


        {
            var d = (
                from ts in _context.teacher_Subjects
                join user in _context.Users on ts.teacher_Id equals user.Id
                
            join ss in _context.class_Subjects on new { sub= ts.subject_Id,sec=ts.class_section } equals new { sub = ss.subject_Id, sec = ss.class_section_Id }
                 join k in _context.class_Sections on ss.class_section_Id equals k.id
                  join s in _context.subjects on ss.subject_Id equals s.id
                   join c in _context.classes on k.class_ID equals c.id
                   join Section in _context.section on k.section_id equals Section.id


                     select new
                     {
                         ts.id,
                   user.FullName,
                     s.title,
                     subject_id=s.id,
                      s.subject_Name,
                       c.level,
                     Section.sectionName
                     }).ToList();

            ;
            return new JsonResult(d);

        }
        [HttpPost("SaveGuardian")]
        public async Task<ActionResult> SaveGuardianAsync([FromForm]Models.GuardianDto guardianDto)

        {
            User user = new User(
              

                )
            { 
                role_id=3,
                Email=guardianDto.Email,
                FullName=guardianDto.FullName,
                UserName=guardianDto.Email,
                PhoneNumber=guardianDto.phone,

            

            };
            var fileName = "";
            var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
            if (guardianDto.profileimg.Length > 0)
            {
                fileName = Guid.NewGuid().ToString().Replace("-", guardianDto.profileimg.FileName) + Path.GetExtension(guardianDto.profileimg.FileName);
                using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                {
                    await guardianDto.profileimg.CopyToAsync(fileStream);
                    //emp.BookPic = fileName;
                }

            }
         user.profilePath = "uploads/" + fileName;

            var result = await _userManager.CreateAsync(user, guardianDto.Password);
            if(result.Succeeded)
            {
                return new JsonResult("Data Saved");
            }

            string error = "";
            foreach(var v in result.Errors)
            {
                error += v.Description;
            }
            return new JsonResult(error);
        }

        [HttpGet("GetGuardian")]
        public async Task<ActionResult> GetGuardian()

        {
            var data = (from k in _context.Users
                        select new
                        {
                          k.role_id,
                         gid=   k.Id,
                            k.FullName,k.Email
                        }).Where(p=>p.role_id==3).ToList();
            return new JsonResult(data);
        }

        //Register Student 
        [HttpPost("SaveStudentData")]
        public async Task<ActionResult> SaveStudentData([FromForm]Models.StudentDto studentDto)

        {
            if (studentDto.sId == 0)
            {
                Random r = new Random();

                int ren = r.Next(4, 121214);
                User user = new User(


                    )
                {
                    role_id = 2,
                    Email = "signup" + ren.ToString() + "@gamail.com",
                    FullName = studentDto.FullName,
                    UserName = "Sugnup" + ren.ToString() + "@gamail.com",




                };
                Student_class cl = new Student_class()
                {
                    class_Section_Id = studentDto.ClassSectionID,

                };
                var fileName = "";
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "uploads");
                if (studentDto.Profileimg.Length > 0)
                {
                    fileName = Guid.NewGuid().ToString().Replace("-", studentDto.Profileimg.FileName) + Path.GetExtension(studentDto.Profileimg.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        await studentDto.Profileimg.CopyToAsync(fileStream);
                        //emp.BookPic = fileName;
                    }

                }
                user.profilePath = "uploads/" + fileName;

                var result = await _userManager.CreateAsync(user, "Abc@124.com!sw1!");
                if (result.Succeeded)
                {

                    Student stu = new Student();
                    stu.userID = user.Id;
                    stu.GuardiaID = studentDto.Guardian;
                    _context.students.Add(stu);
                    _context.SaveChanges();
                    cl.student_Id = user.Id;
                    _context.student_Classes.Add(cl);
                    _context.SaveChanges();
                    return new JsonResult("Data Saved");
                }


                string error = "";
                foreach (var v in result.Errors)
                {
                    error += v.Description;
                }
                return new JsonResult(error);
            }
            else
            {
                

                var student = _context.students.FirstOrDefault(o => o.id == studentDto.sId);
                student.GuardiaID = studentDto.Guardian;
                _context.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                var student_Class = _context.student_Classes.FirstOrDefault(o => o.student_Id == student.userID);
                student_Class.class_Section_Id =studentDto.ClassSectionID;
                _context.Entry(student_Class).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                var user = _context.Users.FirstOrDefault(o => o.Id == student.userID);
                user.FullName = studentDto.FullName;
                _context.SaveChanges();

                return new JsonResult("Data Saved");
            }
        }

        [HttpGet("GetStudent")]
        public async Task<ActionResult> GetStudent()

        {
            var data = (from k in _context.Users
                        join s in _context.students on k.Id equals  s.userID
                        join G in _context.Users on s.GuardiaID equals G.Id
                        join cs  in _context.student_Classes on s.userID equals cs.student_Id
                        join sec in _context.class_Sections on  cs.class_Section_Id equals sec.id
                        join se in _context.section on sec.section_id equals se.id
                        join cls in _context.classes on sec.class_ID equals cls.id

                        select new
                        {
                           cid= cs.class_Section_Id,
                            s.id,
                            k.FullName,
                            s.GuardiaID,
                            k.profilePath,
                            Guardian = G.FullName,
                            clas=cls.level,
                            sec=se.sectionName
                            
                            
                        }).ToList();
            return new JsonResult(data);
        }

        //Class Sift
        [HttpGet("GetClassSectionData")]
        public async Task<ActionResult> GetClassSection()

        {
            var data = (from k in _context.class_Sections
                        join l in _context.classes on k.class_ID equals l.id
                        join m in _context.section on k.section_id equals m.id
                        select new
                        {
                            k.id,
                            l.level,
                            m.sectionName,

                        }).ToList();
            return new JsonResult(data);
        }


        [HttpGet("GetClassSectionStudent")]
        public async Task<ActionResult> GetClassSectionStudent(int id)

        {
            var data = (from k in _context.class_Sections
                        where k.id==id
                        join l in _context.student_Classes on k.id equals l.class_Section_Id
                        join sec in _context.section on k.section_id equals sec.id
                        join cls in _context.classes on k.class_ID equals cls.id
                        join m in _context.Users on l.student_Id equals m.Id
                        select new
                        {
                           sectionID=k.id,
                          sectionleve=cls.level +"-"+sec.sectionName,
                          pormationid=0,
                            studentId=m.Id,
                            studentName=m.FullName,
                            action=false,

                        }).ToList();
            return new JsonResult(data);
        }

        [HttpPost("PromateStudentClass")]
        public async Task<ActionResult> PromateStudentClass([FromBody]List<StudentPromote> list)

        {
           foreach(var e in list)
            {
                if (e.action == true)
                {
                    var d = _context.student_Classes.FirstOrDefault(o => o.student_Id == e.studentId && o.class_Section_Id == e.sectionID);
                    if (d.year_id == year_id)
                    {
                        d.class_Section_Id = e.pormationid;
                        _context.Entry(d).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.SaveChanges();
                    }
                    else
                    {
                        Student_class student_Class = new Student_class();
                        student_Class.student_Id = e.studentId;
                        student_Class.class_Section_Id = e.pormationid;
                        _context.student_Classes.Add(student_Class);
                        _context.SaveChanges();
                    }
                }

            }


            return new JsonResult("");
        }


    }

}