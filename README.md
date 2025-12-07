<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8" />
<meta name="viewport" content="width=device-width, initial-scale=1.0" />
<title>E-Learning Platform API</title>
</head>
<body style="font-family: Arial, sans-serif; line-height: 1.6;">

<h1>📘 E-Learning Platform API</h1>
<p>
A modern and scalable e-learning platform API designed to manage courses, users, exams, and results with clean architecture and role-based access control.
</p>

<hr />

<h2>🚀 Live API Documentation</h2>
<p>
Explore the live API endpoints here:<br />
<a href="http://e-learningnoor.runasp.net/scalar/v1" target="_blank">
  <strong>http://e-learningnoor.runasp.net/scalar/v1</strong>
</a>
</p>

<hr />

<h2>📂 Project Overview</h2>
<p>This project provides a complete backend solution for e-learning systems, including:</p>
<ul>
  <li>Authentication & authorization</li>
  <li>Course management</li>
  <li>Exam creation and submission</li>
  <li>File & image handling</li>
  <li>Stripe payment integration</li>
  <li>HTMLQuestPDF for generating reports</li>
  <li>Results & reporting</li>
  <li>Role-based dashboards (Admin, Instructor, User)</li>
</ul>

<hr />

<h2>🛠️ Tech Stack</h2>
<table border="1" cellpadding="6" cellspacing="0">
  <thead>
    <tr>
      <th>Layer</th>
      <th>Technology</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>Backend</td>
      <td>ASP.NET Core 8 Web API</td>
    </tr>
    <tr>
      <td>ORM</td>
      <td>Entity Framework Core</td>
    </tr>
    <tr>
      <td>Database</td>
      <td>SQL Server</td>
    </tr>
    <tr>
      <td>Authentication</td>
      <td>JWT Tokens + Identity</td>
    </tr>
    <tr>
      <td>Payments</td>
      <td>Stripe</td>
    </tr>
    <tr>
      <td>Reports</td>
      <td>HTMLQuestPDF</td>
    </tr>
    <tr>
      <td>Docs</td>
      <td>Scalar API Docs</td>
    </tr>
  </tbody>
</table>

<hr />
<h2>🗂️ Database ERD</h2>
<p>The following diagram shows the database structure for the LMS system:</p>
<hr/>
<img width="2145" height="3840" alt="Untitled diagram _ Mermaid Chart-2025-09-29-141401" src="https://github.com/user-attachments/assets/0ae0c371-0de5-4436-ba4e-cd2ea42c75e8" />

<hr/>
<h2>📁 Project Architecture</h2>

<pre style="background: #f4f4f4; padding: 12px; border-radius: 6px;">
📦 Project
 ┣ 📂 Course.DAL
 ┣ 📂 Course.BLL
 ┣ 📂 Course.PL
 ┗ 📄 Program.cs
</pre>

<p>Designed using clean architecture principles.</p>

<hr />

<h2>🌟 Features</h2>
<ul>
  <li>User registration & login</li>
  <li>Role-based authorization</li>
  <li>Create, update, and delete courses</li>
  <li>Manage exams and questions</li>
  <li>Submit answers & auto-grade results</li>
  <li>Stripe payments for course enrollment</li>
  <li>Generate PDF reports using HTMLQuestPDF</li>
  <li>Upload images & files</li>
  <li>Error handling & validation</li>
  <li>Scalar API documentation</li>
</ul>

<hr />

<h2>🔧 How to Run Locally</h2>

<h3>1️⃣ Clone the repository</h3>
<pre style="background: #f4f4f4; padding: 12px; border-radius: 6px;">
git clone https://github.com/normaIyad/e-learning
</pre>

<h3>2️⃣ Update <code>appsettings.json</code></h3>
<p>Add your database connection string, JWT key, and Stripe keys.</p>

<h3>3️⃣ Apply migrations</h3>
<pre style="background: #f4f4f4; padding: 12px; border-radius: 6px;">
dotnet ef database update
</pre>

<h3>4️⃣ Run the API</h3>
<pre style="background: #f4f4f4; padding: 12px; border-radius: 6px;">
dotnet run
</pre>

<p>
API default URL: <code>https://localhost:7026/scalar/v1</code>
</p>

<hr />

<h2>🧪 Testing the API</h2>
<p>You can test all endpoints through the live documentation:</p>
<p>
<a href="http://e-learningnoor.runasp.net/scalar/v1" target="_blank">
  <strong>http://e-learningnoor.runasp.net/scalar/v1</strong>
</a>
</p>

<hr />

</body>
</html>
