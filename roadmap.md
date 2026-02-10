## The Project: "CareBridge"

**The Pitch:** A clinical decision support mini-service that identifies patients overdue for screenings (e.g., colonoscopies) and provides a clinician dashboard for initiating automated notifications.

### **The Tech Stack Goal:**

* **Backend:** ASP.NET Core Web API (RESTful service).
* **Frontend:** Angular (SPA with Standalone Components).
* **Logic:** Background processing and LINQ-based clinical rules engine.

---

### **The 7-Day Build Schedule**

| Day | Focus | Task | Documentation Links |
| --- | --- | --- | --- |
| **Day 1** | **Backend Setup** | Scaffold Web API; Create `PatientController` with GET endpoint. | [ASP.NET Web API Controllers](https://learn.microsoft.com/en-us/aspnet/core/web-api/) | [CORS in ASP.NET](https://learn.microsoft.com/en-us/aspnet/core/security/cors) |
| **Day 2** | **LINQ & Logic** | Implement "Screening Engine" logic using LINQ to filter overdue patients. | [C# LINQ Overview](https://learn.microsoft.com/en-us/dotnet/csharp/linq/) | [DateTime Math in .NET](https://learn.microsoft.com/en-us/dotnet/api/system.datetime) |
| **Day 3** | **Angular Core** | Scaffold Angular app; Create `PatientService` using `HttpClient`. | [Angular HttpClient](https://angular.dev/guide/http) | [Angular CLI Reference](https://angular.dev/cli) |
| **Day 4** | **The UI** | Build patient table; use `*ngIf` and `*ngFor` for conditional styling. | [Angular Directives](https://angular.dev/guide/directives) | [Template Syntax](https://angular.dev/guide/templates) |
| **Day 5** | **RxJS Power** | Add search with `debounceTime` and `switchMap` for reactive filtering. | [RxJS Operators](https://rxjs.dev/guide/operators) | [Reactive Forms](https://angular.dev/guide/forms/reactive-forms) |
| **Day 6** | **Healthcare Polish** | Implement a Simulated HIPAA Audit Log via Middleware or Interceptors. | [ASP.NET Core Logging](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) | [Middleware Basics](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/) |
| **Day 7** | **The "Pitch"** | Finalize GitHub README and record a 2-minute Loom demo. | [GitHub Mastering Markdown](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github/basic-writing-and-formatting-syntax) |

---

### **Why this project wins the interview:**

1. **Direct Domain Relevance:** You aren't building a generic "Todo List"; you are building a tool that mimics the core functionality of **mPATH** (Clinical Decision Support).
2. **The "Bridge" Proof:** When asked about the switch from Java/Go to .NET, you can point to your use of **LINQ** as the functional equivalent of Java Streams.
3. **Security-First Mindset:** By including a **Day 6 Audit Log**, you demonstrate that you understand healthcare data requires strict accountability (HIPAA compliance).

---

### **A "Pro" Feature: FHIR Integration**

If you finish early, refactor your `Patient` model to follow the **HL7 FHIR standard**. Instead of simple fields, use the FHIR-compliant structure:

* **Official Spec:** [HL7 FHIR Patient Resource](https://www.hl7.org/fhir/patient.html)
* **Key Change:** Use `Patient.name[0].family` instead of just `LastName`.
