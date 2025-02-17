# HemoTrack

HemoTrack is a comprehensive web application designed to improve the management of sickle cell anemia by streamlining healthcare processes. The application facilitates appointment scheduling, medication tracking, and provides valuable educational resources for patients, doctors, and administrators. Its goal is to empower patients to manage their condition more effectively while reducing the administrative burden on healthcare providers.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture & Technology Stack](#architecture--technology-stack)
- [Installation & Setup](#installation--setup)
- [Usage](#usage)
- [Contributing](#contributing)


## Overview

Sickle cell anemia is a chronic genetic blood disorder characterized by abnormal hemoglobin, which complicates treatment adherence and health tracking. HemoTrack addresses these challenges by providing:

- **Automated Scheduling:** A digital solution that replaces manual appointment booking with a streamlined, error-resistant process.
- **Educational Resources:** An integrated blog and resource center that offers the latest information on disease management and lifestyle recommendations.
- **Role-based Interactions:** Customized dashboards for patients, doctors, and administrators to ensure that each user accesses the features most relevant to them.

### ER Diagram for the web application database
![ER Diagram for the web application database](https://github.com/user-attachments/assets/07008ae4-0ffd-48bd-884f-531af5cb04ff)

## Features

- **Appointment Management**
  - Schedule, reschedule, and cancel appointments easily.
  - Reduce human errors such as double bookings or missed appointments.
  
- **User Dashboards**
  - **Patients:** View upcoming appointments, access personalized health resources, and manage their schedules.
  - **Doctors:** Monitor patient appointments, manage their schedules, and review patient details.
  - **Administrator:** Oversee user roles, manage patient and doctor accounts, and control access to educational content.
  
- **Educational Blog**
  - Provides up-to-date articles and resources on sickle cell anemia, treatment options, and wellness tips.
  - Admin-managed content that can be added, edited, or removed as needed.
  
- **Security & Privacy**
  - Role-based access control (RBAC) ensures that each user sees only the data pertinent to their role.
  - Robust login mechanisms with error handling (e.g., “Invalid login attempt” and “User is not registered” notifications) to safeguard sensitive healthcare data.

### Use-case diagram for the web application
![Use-case diagram for the web application](https://github.com/user-attachments/assets/0c47f8ff-589e-4547-abe6-72df5dba8a7e)

## Architecture & Technology Stack

HemoTrack is built using a modern web development stack that balances robust back-end functionality with a dynamic, user-friendly front end.

- **Front-End:**  
  - **Languages & Frameworks:** JavaScript, HTML, CSS, Bootstrap  
  - **User Experience:** Intuitive interfaces for easy navigation and minimal training requirements.

- **Back-End:**  
  - **Framework:** ASP.NET Core MVC (C#)  
  - **Databases:**  
    - **Microsoft SQL Server:** For handling structured, relational data such as user information and appointment records.  
    - **MongoDB:** For flexible storage of unstructured educational content, including multimedia blog posts.

- **Development Tools:**  
  - **IDE:** Visual Studio Code  
  - **Version Control:** Git (for source code management and collaborative development)



## Installation & Setup

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [MongoDB](https://www.mongodb.com/try/download/community)
- [Visual Studio Code](https://code.visualstudio.com/) or another preferred IDE
- Git (for version control)

### Installation Steps

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/AlvinKimata/HemoTrack.git
   cd HemoTrack
   ```

2. **Configure the Database:**
   - Set up Microsoft SQL Server and create the necessary database and tables for users, appointments, and other relational data.
   - Configure MongoDB for the storage of educational content.
   - Update the connection strings in the application’s configuration file (e.g., `appsettings.json`).

3. **Restore Dependencies and Build:**

   ```bash
   dotnet restore
   dotnet build
   ```

4. **Run the Application:**

   ```bash
   dotnet run
   ```

5. **Access the Application:**
   - Open a browser and navigate to `http://localhost:5000` (or the configured port) to use HemoTrack.

## Usage

### User Roles and Access

- **Patient:**
  - Register and create a profile.
  - Log in to view the personalized dashboard.
  - Schedule and manage appointments.
  - Access educational materials regarding sickle cell anemia management.

- **Doctor:**
  - Log in to view and manage patient appointments.
  - Access patient details relevant to the appointment schedule.

- **Administrator:**
  - Manage user accounts by registering, modifying, or deleting patient and doctor profiles.
  - Oversee and update educational content via the blog feature.
  - Utilize the admin dashboard for role-based management and system monitoring.

### Navigating the Dashboards

- The **Patient Dashboard** provides an overview of upcoming appointments and links to educational resources.
- The **Doctor Dashboard** focuses on managing appointment schedules and reviewing patient details.
- The **Administrator Dashboard** offers control over user roles, permissions, and content management, ensuring smooth operation of the web application.



## Contributing

Contributions to HemoTrack are welcome. If you wish to improve the application or add new features, please follow these guidelines:

1. Fork the repository.
2. Create a new branch for your feature or bug fix.
3. Make your changes and test thoroughly.
4. Submit a pull request detailing your changes and the rationale behind them.
