
# Business Manager

ASP.NET group project for the programme IT-Kariera. A web application for managing a business. The functionalities include:
- Users
- Roles
- Teams
- Projects
- Time-off Requests 

### Developers:
- [Boyan Apostolov](https://github.com/Boyan-Apostolov)
- [Teodor Ivanov](https://github.com/m0nSteRX7)

# 🛠 Built with:

-   [ASP.NET Core](https://github.com/dotnet/aspnetcore)
-   [Entity Framework Core](https://github.com/dotnet/efcore)
-   [AutoMapper](https://github.com/AutoMapper/AutoMapper)
-   [Bootstrap](https://github.com/twbs/bootstrap)
-   [DropBox API](https://www.dropbox.com/developers)
-   [jQuery](https://jquery.com/)
-   [SweetAlert2](https://github.com/sweetalert2/sweetalert2)

# Seeded Users:
| **User with role** |Username                 |Password         |
|-------------------|--------------------------|-----------------|
|CEO			    |admin                     |Admin123         | 
|Team_Lead          |team-lead                 |Team-lead123     | 
|Developer		    |dev                       | Developer123    |
|New-User		    |new-user                  | New-user123     |

# Permissions by Role:

| **Permissions**            | CEO | Team_Lead | Developer/Unassigned |
| -------------------------- | --- | --------- | -------------------- |
| Lading page                | ✅ | ✅        | ✅                   |
| All Users                  | ✅ | ✅        | ✅                   |
| Users CRUD                 | ✅ | ❌        | ❌                   |
| All Roles                  | ✅ | ✅        | ✅                   |
| Roles CRUD                 | ✅ | ❌        | ❌                   |
| All Projects               | ✅ | ✅        | ✅                   |
| Projects CRUD              | ✅ | ✅        | ❌                   |
| All Teams                  | ✅ | ✅        | ✅                   |
| Teams CRUD                 | ✅ | ✅        | ❌                   |
| All Requests               | ✅ | ✅        | ✅                   |
| Requests (Create/Edit)     | ✅ | ✅        | ✅                   |
| Requests (Approve/Decline)  | ✅ (all) | ✅ (team) | ❌                   |

# Pages:

**Landing page**

The landing page of our web app
![Landing page](https://i.ibb.co/qg2Bp9F/01-landing.png)

**Login/Register pages**

The user needs to provide a First Name, Last Name, Username and Email to register.
![Login/Register pages](https://i.ibb.co/6JYWWZx/05-login-register.png)

**All Users page**

Paginated and filtered page of all users on the platform. The CEO has access to the Edit and Delete buttons.
![All Users page](https://i.ibb.co/HxFr8wz/02-users-all.png)

**Create/Edit User page**

The CEO can create new users and simultaniously assign them to a role.
![Create/Edit User page](https://i.ibb.co/wdKMjZx/03-users-create-edit.png)

**User Details page**

Information about the users, as well as a direct link to the assigned team, if applicable.
![User details](https://i.ibb.co/7kj2BJF/user-details.png)

The user can be assigned to a dynamically on the same page, using the SweetAlert2 and jQuery libraries:
![Assign to team](https://i.ibb.co/Y2BkdQV/select-team.png)

**Delete User page**

A confirmation page when the CEO tries to delete a user
![Delete User page](https://i.ibb.co/3htFyL6/user-delete.png)

**All Roles page**

Paged and filterable page with all registered roles, with quick access links and information about the number of assigned users
![All Roles page](https://i.ibb.co/VCc5H6z/all-roles.png)

**Create/Edit Role page**

Pages for Creating new and Editing existing roles. There is only one property - Name of the role. 
![Create/Edit Role page](https://i.ibb.co/6HdmKgD/roles-crud.png)

**Role Details page**

Details page for the role, which shows the users that are assigned to the said role, and quick access links to them.
![Role Details page](https://i.ibb.co/WfPyH1J/role-details.png)

**Delete Role page**

A confirmation page for deleting a role.
![Delete Role page](https://i.ibb.co/6w6Z6c4/role-delete.png)

**All Teams page**

Paginated and filterable page with all created teams, showing the count their assigned projects and users
![All Teams page](https://i.ibb.co/NKrm9CN/all-teams.png)

**Create/Edit Team page**

Simple pages for Creating and Editing teams
![Create/Edit Team page](https://i.ibb.co/JKbfBqP/teams-crud.png)

**Team Details page**

The details page contains all information we store about a team. Here you can find the users, assigned to the team and quick links for them. Team leaders and CEO users can unassign users from the team using the red button. For easier visual purpoces, team leaders and developers have special icons next to their names. On the bottom of the page, we can see all projects, the team is working on, along with their status and link.
![Team Details page](https://i.ibb.co/mbQ8ws1/team-details.png)

**Delete Team page**

Confirmation page when trying to delete a team.
![Delete Team page](https://i.ibb.co/W0Yb9Pp/delete-team.png)

**All Projects page**

This paginated and filterable page shows us all teams, along with their current status and priority. CEO and Team leaders have access for editing and deleting projects.
![All Projects page](https://i.ibb.co/SRhbSJx/projects-all.png)

**Create/Edit Project page**

Pages for creating and editing projects. The status and priority are chosen using the dropdown menus
![Create/Edit Project page](https://i.ibb.co/kmhKX2T/projects-crud.png)

**Project Details page**

This page shows us all info we store about the project, along with the corresponding assigned team.
![Project Details page](https://i.ibb.co/xmWHQ79/project-details.png)

**Delete Project page**

A confirmation page for deleting a project.
![Delete Project page](https://i.ibb.co/gFCFvCS/project-delete.png)

** All Requests page**

This paginated and filterable by date page shows the CEO all time-off requests and shows the team leaders only the requests about users in their team. 
The Approve, Decline and Delete buttons are all dynamic, using SweetAlert2 and jQuery and helps the user do their actions without leaving the page!
![All Requests page](https://i.ibb.co/CBV0phf/time-off-all.png)

**Create/Edit Request page**

Simple pages for creating and editing time-off requests. If the "Sick" option is chosen, a file input box appears uploading the correct document. The 'To date' field is hidden when choosing "Half day?"
![Create/Edit Request page](https://i.ibb.co/BwNw9W8/time-off-crud.png)
