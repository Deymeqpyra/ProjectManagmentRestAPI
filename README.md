# **Project Collaboration Tool**

## **Overview**

The **Project Collaboration Tool** is a powerful project management and collaboration platform designed to help teams streamline their workflows. It enables users to manage projects, tasks, and teams effectively, with a rich set of features and seamless integration between the backend and frontend.

---

## **Features**

### **Key Functionalities**
- **User Management**: Register, manage profiles, and join projects based on roles.
- **Project Management**: Create, update, delete projects, assign team members, and track status.
- **Task Management**: Create, assign, update tasks, and manage progress with clear task categorization.
- **Commenting System**: Facilitate discussions with threaded comments on tasks and projects.
- **Role-Based Assignments**: Assign tasks based on user roles (e.g., Developer, Team Lead).
- **Many-to-Many Relationships**:
  - Users ↔ Projects: Collaborate on multiple projects.
  - Users ↔ Tasks: Work on tasks that align with assigned roles.
  - Projects ↔ Tags: Tag projects with relevant keywords for categorization.
- **Frontend Dashboard**: React-based dashboard showcasing real-time updates of tasks, projects, and team members.

### **Entity Relationships**
- **Users**:
  - Many-to-Many ↔ Roles
  - Many-to-Many ↔ Tasks (dependent on project membership)
  - Many-to-One ↔ Projects (one active project at a time)
- **Projects**:
  - Many-to-Many ↔ Tasks
  - Many-to-Many ↔ Tags
  - Many-to-One ↔ Status
  - Many-to-One ↔ Priorities
- **Tasks**:
  - Many-to-Many ↔ Users
  - One-to-Many ↔ Categories
- **Comments**:
  - Belongs to Users and Projects.

---

## **Technologies Used**

- **Backend**:  
  - **C# with ASP.NET Core Web API**  
    RESTful API architecture with support for role-based authorization.
- **Frontend**:  
  - **React**  
    Interactive and real-time user interface with state management.
- **Database**:  
  - **Entity Framework Core**  
    Relational database schema with comprehensive seed data.

---

## **Database Schema**

### **Tables**

#### **User**
| Column            | Type    | Description                                |
|--------------------|---------|--------------------------------------------|
| Name              | String  | Full name of the user.                     |
| Email             | String  | Email for login and notifications.         |
| Password (hashed) | String  | Securely stored password.                  |
| RoleId            | FK      | Role of the user.                          |
| ProjectId         | FK      | Active project associated with the user.   |

#### **Project**
| Column            | Type    | Description                                |
|--------------------|---------|--------------------------------------------|
| Title             | String  | Project name.                              |
| Description       | String  | Detailed information about the project.    |
| LastUpdate        | Date    | Timestamp of the last update.              |
| StatusId          | FK      | Current status of the project.             |
| PriorityId        | FK      | Priority level for the project.            |

#### **Task**
| Column            | Type    | Description                                |
|--------------------|---------|--------------------------------------------|
| Title             | String  | Task name.                                 |
| Description       | String  | Short task description.                    |
| IsFinished        | Bool    | Status of the task.                        |
| CategoryId        | FK      | Categorization for task grouping.          |

#### **Comment**
| Column            | Type    | Description                                |
|--------------------|---------|--------------------------------------------|
| Content           | String  | Text content of the comment.               |
| UserId            | FK      | User who created the comment.              |
| ProjectId         | FK      | Project associated with the comment.       |
| WhenWasPosted     | Date    | Timestamp of when the comment was posted.  |

#### **Other Tables**
- **Tags**: Tagging system for projects and tasks.
- **Categories**: Task grouping categories.
- **Status**: Project and task status values.
- **Roles**: Roles like Developer, Team Lead, etc.

---

## **Development Progress**

### **Implemented Features**
- [x] Entities:  
  - User  
  - Project  
  - Task  
  - Tag  
  - Comment  
  - Priorities  
  - Categories  
  - Status  
  - Role  
- [x] Data Seed: Populated the database with test data.
- [x] Role-Based Access Control: Ensure only project creators can modify entities.
- [x] Password Hashing: Secure password storage.

### **To-Do**
- [ ] **SignalR Integration**: Real-time updates for tasks and project changes.

---

Make teamwork seamless and efficient with **Project Collaboration Tool**! 🎉
