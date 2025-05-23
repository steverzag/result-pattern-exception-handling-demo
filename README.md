# 📘 ResultPatternExceptionHandlingDemo

This project demonstrates and compares two common approaches for handling errors in .NET Web APIs:

---

## 🧭 Error Handling Strategies

### 1. **Exception-based Handling (Throw + Middleware)**
Errors are thrown as exceptions and handled globally through middleware. This is the traditional approach in many .NET applications, offering centralized error logging and standardized HTTP responses.

### 2. **Result Pattern**
This approach avoids throwing exceptions in favor of returning structured result objects (e.g., `Result<T>`, `ErrorResult`). It promotes explicit error handling and encourages a more functional, predictable flow of control.

---

## 🎯 Purpose

The goal of this project is to serve as a reference and experimentation ground to evaluate the **pros, cons, and practical differences** between these two patterns. It can help teams decide which strategy best fits their coding style, performance needs, and maintainability goals.

## 🛠️ Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Grafana k6](https://grafana.com/docs/k6/latest/set-up/install-k6/)

## 📦 Installation
1. Clone this repository:
   ```sh
   git clone https://github.com/steverzag/result-pattern-exception-handling-demo
   cd <repository-folder>
   ```
2. Restore dependencies:
   ```sh
   dotnet restore
   ```
3. Build the application:
   ```sh
   dotnet build
   ```


## ▶️ Running the Application
To run the application locally:
   ```sh
   dotnet run --project ./ResultPatternExceptionHandlingDemo.API/ResultPatternExceptionHandlingDemo.API.csproj
   ```

Or to run the application using [Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview)
   ```
   dotnet run --project ./ResultPatternExceptionHandlingDemo.AppHost/ResultPatternExceptionHandlingDemo.AppHost.csproj
   ```

By default, the application will be available at `http://localhost:5000` (or `https://localhost:5001` for HTTPS).

## 🚀 Usage

The application exposes HTTP endpoints that demonstrate and compare different error handling strategies. You can interact with these endpoints to observe how each approach behaves in success and failure scenarios.

For detailed request samples and expected responses, refer to the `.http` file: 
./ResultPatternExceptionHandlingDemo.API/ResultPatternExceptionHandlingDemo.API.http


### 🧪 Error Handling Variants Demonstrated:

- **`throw`** – Traditional exception throwing, handled by middleware
- **`fail`** – Custom Result pattern implementation
- **`language-ext`** – Result pattern using the [LanguageExt](https://github.com/louthy/language-ext) NuGet package
- **`fluent-results`** – Result pattern using the [FluentResults](https://github.com/altmann/FluentResults) NuGet package

---

### 📈 Performance Testing

You can run performance/load tests using [k6](https://k6.io/) scripts included in the project root.

Example command:
```sh
k6 run apitest-v1.create-user.js

Feel free to explore other apitest-v#.*.js files for additional test cases.

## 📊 Performance Comparison (k6 Load Test)

| Variant             | Avg Duration | P95 Duration | Iterations   | Iterations/sec | Requests/sec | Failure Rate | Notes                                |
|---------------------|--------------|--------------|--------------|----------------|---------------|--------------|--------------------------------------|
| `v1` - Throw        | **2.01 ms**  | 4.15 ms      | 269,391      | ~4,490/s       | ~8,979/s      | 50%          | Exception + Middleware (slowest)     |
| `v2` - Fail         | **710 µs**   | 1.24 ms      | 751,206      | ~12,520/s      | ~25,040/s     | 50%          | Custom result pattern handling      |
| `v3` - LanguageExt  | **878 µs**   | 1.85 ms      | 603,622      | ~10,060/s      | ~20,120/s     | 50%          | [LanguageExt](https://github.com/louthy/language-ext) result handling  |
| `v4` - FluentResult | **760 µs**   | 1.34 ms      | 701,291      | ~11,688/s      | ~23,376/s     | 50%          | [FluentResults](https://github.com/altmann/FluentResults) result handling                |

> ⚠️ All tests simulate a 50% failure rate to benchmark error handling overhead.

---

### 🔎 Observations

- **v1 (Exception-based)** is **~2.5× slower** in average duration and supports **~50% fewer requests/sec** than the result-based implementations.
- **Custom result pattern (`v2`)** shows the best performance, with the **lowest average and P95 duration**, and **highest request throughput**.
- **LanguageExt (`v3`)** is slightly behind FluentResults but still much faster than exceptions.
- **FluentResults (`v4`)** performs closely to Custom result pattern.

> 💡 Note: While these results clearly show performance differences, they are based on a specific test environment and workload. Real-world outcomes may vary depending on factors like I/O, serialization, validation logic, or infrastructure setup.

---

### 🏁 Conclusion

✅ **Avoid using exceptions** for expected control flow.  
✅ **Use a result pattern (like `FluentResults` or `LanguageExt`)** for scalable, fast API error handling.

