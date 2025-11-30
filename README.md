# School Management System API Documentation

## Overview

This document provides comprehensive documentation for the School Management System API. The API is organized by user roles (Admin, Teacher, Student) and includes lookup endpoints for common data access.

## Role & Responsibilities

### Admin

Admins have full system access and are responsible for:
- Managing departments (create, update, delete)
- Managing courses (create, update, delete)
- Managing users (create, update, delete, view all users)
- Viewing all classes across the system
- Viewing paginated lists of students and classes

### Teacher

Teachers manage their assigned classes and students:
- Creating and managing their own classes
- Enrolling students in their classes
- Viewing enrollments for their classes
- Marking student attendance
- Creating assignments for their classes
- Grading student submissions
- Sending notifications to students
- Viewing their own assignments with pagination

### Student

Students interact with their enrolled classes:
- Viewing their enrolled classes
- Viewing assignments for their enrolled classes
- Submitting assignments
- Viewing their submission grades
- Viewing their attendance records
- Viewing notifications sent to them

## Authentication

All endpoints except registration require authentication via JWT Bearer token. Include the token in the Authorization header:
```
Authorization: Bearer <token>
```

Base URL: `https://localhost:44391/`

---

## Authentication APIs

### Register
- **Method:** POST
- **URL:** `api/auth/register`
- **Body:**
```json
{
  "name": "string",
  "email": "string",
  "password": "string",
  "role": "Admin" | "Teacher" | "Student"
}
```
- **Purpose:** Register a new user (Admin, Teacher or Student). Admin users must be created by another admin.

### Login
- **Method:** POST
- **URL:** `api/auth/login`
- **Body:**
```json
{
  "email": "string",
  "password": "string"
}
```
- **Purpose:** Authenticate user and receive JWT token and refresh token.

### Refresh Token
- **Method:** POST
- **URL:** `api/auth/refresh-token`
- **Body:**
```json
{
  "refreshToken": "string"
}
```
- **Purpose:** Get a new access token using a valid refresh token.

---

## Admin APIs

### Departments

#### Create Department
- **Method:** POST
- **URL:** `api/admin/departments`
- **Body:**
```json
{
  "name": "string",
  "description": "string",
  "headOfDepartmentId": 0
}
```
- **Purpose:** Create a new department. Head of Department must be a Teacher.

#### Update Department
- **Method:** PUT
- **URL:** `api/admin/departments`
- **Body:**
```json
{
  "id": 0,
  "name": "string",
  "description": "string",
  "headOfDepartmentId": 0
}
```
- **Purpose:** Update an existing department.

#### Delete Department
- **Method:** DELETE
- **URL:** `api/admin/departments/{id}`
- **Purpose:** Soft delete a department.

### Courses

#### Create Course
- **Method:** POST
- **URL:** `api/admin/courses`
- **Body:**
```json
{
  "name": "string",
  "code": "string",
  "description": "string",
  "departmentId": 0,
  "credits": 0
}
```
- **Purpose:** Create a new course. Course code must be unique per department.

#### Update Course
- **Method:** PUT
- **URL:** `api/admin/courses`
- **Body:**
```json
{
  "id": 0,
  "name": "string",
  "code": "string",
  "description": "string",
  "departmentId": 0,
  "credits": 0
}
```
- **Purpose:** Update an existing course.

#### Delete Course
- **Method:** DELETE
- **URL:** `api/admin/courses/{id}`
- **Purpose:** Soft delete a course.

### Users

#### Create User
- **Method:** POST
- **URL:** `api/admin/users`
- **Body:**
```json
{
  "name": "string",
  "email": "string",
  "password": "string",
  "role": "Admin" | "Teacher" | "Student"
}
```
- **Purpose:** Create a new user with any role.

#### Get All Users
- **Method:** GET
- **URL:** `api/admin/users`
- **Purpose:** Retrieve all users in the system.

#### Get User By ID
- **Method:** GET
- **URL:** `api/admin/users/{id}`
- **Purpose:** Retrieve a specific user by ID.

#### Get Users By Role
- **Method:** GET
- **URL:** `api/admin/users/role/{role}`
- **Purpose:** Retrieve all users with a specific role (Admin, Teacher, or Student).

#### Update User
- **Method:** PUT
- **URL:** `api/admin/users`
- **Body:**
```json
{
  "id": 0,
  "name": "string",
  "role": "Admin" | "Teacher" | "Student"
}
```
- **Purpose:** Update user name and role. Email cannot be changed.

#### Delete User
- **Method:** DELETE
- **URL:** `api/admin/users/{id}`
- **Purpose:** Soft delete a user.

#### Get Paged Students
- **Method:** GET
- **URL:** `api/admin/users/students/paged?PageNumber=1&PageSize=10`
- **Purpose:** Retrieve paginated list of all students.

### Classes

#### Get All Classes
- **Method:** GET
- **URL:** `api/admin/classes`
- **Purpose:** Retrieve all classes in the system.

#### Get Class By ID
- **Method:** GET
- **URL:** `api/admin/classes/{id}`
- **Purpose:** Retrieve a specific class by ID.

#### Get Paged Classes
- **Method:** GET
- **URL:** `api/admin/classes/paged?PageNumber=1&PageSize=10`
- **Purpose:** Retrieve paginated list of all classes.

---

## Teacher APIs

### Classes

#### Create Class
- **Method:** POST
- **URL:** `api/teacher/classes`
- **Body:**
```json
{
  "name": "string",
  "courseId": 0,
  "semester": "string",
  "startDate": "2025-01-01T00:00:00Z",
  "endDate": "2025-12-31T00:00:00Z",
  "isActive": true
}
```
- **Purpose:** Create a new class. The authenticated teacher is automatically assigned as the class teacher.

#### Get All Classes
- **Method:** GET
- **URL:** `api/teacher/classes`
- **Purpose:** Retrieve all classes assigned to the authenticated teacher.

#### Get Class By ID
- **Method:** GET
- **URL:** `api/teacher/classes/{id}`
- **Purpose:** Retrieve a specific class by ID. Only returns classes assigned to the authenticated teacher.

#### Update Class
- **Method:** PUT
- **URL:** `api/teacher/classes`
- **Body:**
```json
{
  "id": 0,
  "name": "string",
  "courseId": 0,
  "semester": "string",
  "startDate": "2025-01-01T00:00:00Z",
  "endDate": "2025-12-31T00:00:00Z",
  "isActive": true
}
```
- **Purpose:** Update a class. Only the assigned teacher can update their own classes.

#### Deactivate Class
- **Method:** PUT
- **URL:** `api/teacher/classes/{id}/deactivate`
- **Purpose:** Deactivate a class. Only the assigned teacher can deactivate their own classes.

#### Activate Class
- **Method:** PUT
- **URL:** `api/teacher/classes/{id}/activate`
- **Purpose:** Activate a class. Only the assigned teacher can activate their own classes.

#### Enroll Student
- **Method:** POST
- **URL:** `api/teacher/classes/{classId}/enroll`
- **Body:**
```json
{
  "studentId": 0
}
```
- **Purpose:** Enroll a student in a class. Only the assigned teacher can enroll students in their classes.

#### Get Enrollments
- **Method:** GET
- **URL:** `api/teacher/classes/{classId}/enrollments`
- **Purpose:** Retrieve all student enrollments for a specific class. Only the assigned teacher can view enrollments.

### Attendance

#### Mark Attendance
- **Method:** POST
- **URL:** `api/teacher/attendances`
- **Body:**
```json
{
  "classId": 0,
  "studentId": 0,
  "date": "2025-01-01T00:00:00Z",
  "status": "Present" | "Absent" | "Late"
}
```
- **Purpose:** Mark attendance for a student in a class. Only the assigned teacher can mark attendance.

#### Get Attendance History
- **Method:** GET
- **URL:** `api/teacher/attendances/{classId}`
- **Purpose:** Retrieve attendance history for all students in a specific class. Only the assigned teacher can view attendance.

### Assignments

#### Create Assignment
- **Method:** POST
- **URL:** `api/teacher/assignments`
- **Body:**
```json
{
  "classId": 0,
  "title": "string",
  "description": "string",
  "dueDate": "2025-01-01T00:00:00Z"
}
```
- **Purpose:** Create an assignment for a class. Only the assigned teacher can create assignments.

#### Get Assignments By Class ID
- **Method:** GET
- **URL:** `api/teacher/assignments/{classId}`
- **Purpose:** Retrieve all assignments for a specific class. Only the assigned teacher can view assignments.

#### Grade Submission
- **Method:** POST
- **URL:** `api/teacher/assignments/{submissionId}/grade`
- **Body:**
```json
{
  "grade": 0,
  "remarks": "string"
}
```
- **Purpose:** Grade a student submission. Only the assigned teacher can grade submissions for their classes.

#### Get Paged Assignments
- **Method:** GET
- **URL:** `api/teacher/assignments/paged?PageNumber=1&PageSize=10`
- **Purpose:** Retrieve paginated list of all assignments created by the authenticated teacher.

### Notifications

#### Create Notification
- **Method:** POST
- **URL:** `api/teacher/notifications`
- **Body:**
```json
{
  "classId": 0 (optional),
  "studentIds": [0] (optional),
  "recipientId": 0 (optional),
  "title": "string",
  "message": "string"
}
```
- **Purpose:** Send notification to students. Can send to all students in a class, specific students, or a single student. Must provide one of: classId, studentIds, or recipientId.

#### Get All Notifications
- **Method:** GET
- **URL:** `api/teacher/notifications`
- **Purpose:** Retrieve all notifications sent by the authenticated teacher.

---

## Student APIs

### Assignments

#### Get Assignment By ID
- **Method:** GET
- **URL:** `api/student/assignments/{id}`
- **Purpose:** Retrieve a specific assignment. Only returns assignments for classes the student is enrolled in.

#### Submit Assignment
- **Method:** POST
- **URL:** `api/student/assignments/{assignmentId}/submit`
- **Body:** Form-data with file upload
  - `file`: File to upload
- **Purpose:** Submit an assignment. Only students enrolled in the class can submit. Cannot submit after due date or if already submitted.

#### Get Submission By ID
- **Method:** GET
- **URL:** `api/student/assignments/submissions/{submissionId}`
- **Purpose:** Retrieve a specific submission. Students can only view their own submissions.

#### Get Grades
- **Method:** GET
- **URL:** `api/student/assignments/grades`
- **Purpose:** Retrieve all submissions with grades for the authenticated student.

### Classes

#### Get Enrolled Classes
- **Method:** GET
- **URL:** `api/student/classes`
- **Purpose:** Retrieve all classes the authenticated student is enrolled in.

### Attendance

#### Get Attendance
- **Method:** GET
- **URL:** `api/student/attendances`
- **Purpose:** Retrieve all attendance records for the authenticated student.

#### Get Attendance By Class ID
- **Method:** GET
- **URL:** `api/student/attendances/{classId}`
- **Purpose:** Retrieve attendance records for a specific class. Only returns if the student is enrolled in the class.

### Notifications

#### Get All Notifications
- **Method:** GET
- **URL:** `api/student/notifications`
- **Purpose:** Retrieve all notifications sent to the authenticated student.

#### Get Notification By ID
- **Method:** GET
- **URL:** `api/student/notifications/{id}`
- **Purpose:** Retrieve a specific notification and mark it as read. Students can only view their own notifications.

---

## Lookup APIs

Lookup APIs provide read-only access to common data that can be viewed by any authenticated user (Admin, Teacher, or Student). These endpoints are used for dropdowns, reference data, and general information lookup.

### Courses

#### Get All Courses
- **Method:** GET
- **URL:** `api/lookup/courses`
- **Purpose:** Retrieve all active courses. Accessible by any authenticated user.

#### Get Course By ID
- **Method:** GET
- **URL:** `api/lookup/courses/{id}`
- **Purpose:** Retrieve a specific course by ID. Accessible by any authenticated user.

### Departments

#### Get All Departments
- **Method:** GET
- **URL:** `api/lookup/departments`
- **Purpose:** Retrieve all active departments. Accessible by any authenticated user.

#### Get Department By ID
- **Method:** GET
- **URL:** `api/lookup/departments/{id}`
- **Purpose:** Retrieve a specific department by ID. Accessible by any authenticated user.

---

## Multiple Controllers with Same Name

The API uses multiple controllers with the same name (e.g., ClassesController, CoursesController) in different folders (Admin, Teacher, Student, Lookup). This design pattern is intentional and serves several important purposes:

### Access Control and Security

Different roles have different access levels to the same entities. For example:
- **Admin ClassesController** (`api/admin/classes`): Admins can view all classes in the system
- **Teacher ClassesController** (`api/teacher/classes`): Teachers can only view and manage their own classes
- **Student ClassesController** (`api/student/classes`): Students can only view classes they are enrolled in

By separating these into different controllers, we ensure that:
1. Each controller enforces role-specific authorization at the controller level using `[Authorize(Roles = "RoleName")]`
2. Business logic in services can filter data based on the authenticated user's role
3. Clear separation of concerns - each role's endpoints are isolated

### Different Functionality

Even though controllers share the same name, they often expose different operations:
- **Admin CoursesController** (`api/admin/courses`): Create, Update, Delete courses
- **Lookup CoursesController** (`api/lookup/courses`): Only Get operations (read-only)

This prevents unauthorized users from accessing operations they shouldn't have, even if they somehow bypass route-level authorization.

### Clear API Structure

The folder structure (`Features/Admin`, `Features/Teacher`, `Features/Student`, `Features/Lookup`) makes it immediately clear:
- Which role can access which endpoints
- Where to find and modify role-specific functionality
- How the API is organized for different user types

### Maintainability

This approach makes the codebase easier to maintain:
- Changes to admin functionality don't affect teacher or student endpoints
- Each controller can evolve independently
- Testing is simpler as each controller can be tested in isolation
- Code reviews are easier as changes are scoped to specific roles

---

## Error Handling

The API uses custom exceptions that return appropriate HTTP status codes:

- **NotFoundException** (404): Resource not found
- **BusinessException** (400): Business rule violation
- **UnauthorizedException** (401): Authentication/authorization failure
- **ValidationException** (400): Input validation errors
- **500 Internal Server Error**: Unexpected errors

All error responses follow this format:
```json
{
  "success": false,
  "message": "Error message"
}
```

For validation errors:
```json
{
  "success": false,
  "errors": {
    "fieldName": ["Error message 1", "Error message 2"]
  }
}
```

## Swagger Documentation

Swagger UI is available at: **https://localhost:44391/swagger**

## Database Setup

1. Add your database connection string in the appropriate configuration file:
   - For Development: `appsettings.Development.json`
   - For Production: `appsettings.Production.json`
   - Generally: `appsettings.json`

   Connection string format:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=SchoolManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

2. Open Package Manager Console in Visual Studio and set the default project to `School.Infrastructure`.

3. Execute the following command to create the initial migration:
   ```
   add-migration InitialMigration
   ```

4. Execute the following command to create the database and tables:
   ```
   update-database
   ```

The database and all tables will be created automatically.

---




## Notes

- All dates should be in ISO 8601 format (UTC)
- File uploads use multipart/form-data
- Pagination parameters: PageNumber (1-based) and PageSize
- Soft delete is used - deleted entities are marked as deleted but not removed from database
- All endpoints require authentication except registration
- Role-based authorization is enforced at the controller level

