## The Project: "CareBridge"

**The Pitch:** A clinical decision support (CDS) mini-service that identifies patients overdue for screenings (e.g., colonoscopies) and provides a clinician dashboard for initiating automated notifications.

### **Tech Stack**

- **Backend:** ASP.NET Core 10 Web API (RESTful).
- **Frontend:** Angular 21.
- **Logic:** Background processing and LINQ-based clinical rules engine.

---

### **The 7-Day Build Schedule**

| Day       | Focus              | Task                                                                      | Key Concept                    |
| --------- | ------------------ | ------------------------------------------------------------------------- | ------------------------------ |
| **Day 1** | **Backend Setup**  | Scaffold Web API; Create `PatientController` with GET/POST.               | **Minimal APIs / Controllers** |
| **Day 2** | **LINQ & Logic**   | Implement "Screening Engine" logic using LINQ to filter overdue patients. | **C# Functional Logic**        |
| **Day 3** | **Angular Core**   | Scaffold App; Setup `provideExperimentalZonelessChangeDetection()`.       | **Zoneless Angular**           |
| **Day 4** | **Signal State**   | Use `signal()`, `computed()`, and `@for` to build the patient dashboard.  | **Fine-grained Reactivity**    |
| **Day 5** | **Modern Input**   | Use `linkedSignal` for search/filter logic.                               | **Signal-based UX**            |
| **Day 6** | **Security/Audit** | Implement a Functional Interceptor to log HIPAA-compliant access events.  | **Functional Interceptors**    |
| **Day 7** | **The Pitch**      | Finalize README; demo the speed of Zoneless Angular updates.              | **Performance Metrics**        |
