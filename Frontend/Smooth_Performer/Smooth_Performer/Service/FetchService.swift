//
//  FetchData.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import Foundation

struct FetchService{
    private enum FetchError: Error {
        case badResponse
    }
    
    init() {
        fetchStudentJson()
    }
    
    private(set) var student:Student?
    private(set) var course:Course?
    
    private let baseURL = URL(string: "https://your-app-868428109250.us-central1.run.app")
    
    func fetchCurrentStudent() throws -> Student?{
        if let data = UserDefaults.standard.data(forKey: "savedStudent") {
                let decoder = JSONDecoder()
                do {
                    let student = try decoder.decode(Student.self, from: data)
                    return student
                } catch {
                    print("Failed to load student: \(error)")
                }
            }
            return nil
    }
    
    func saveStudentToUserDefaults(student: Student) {
        let encoder = JSONEncoder()
        do {
            let data = try encoder.encode(student)
            UserDefaults.standard.set(data, forKey: "savedStudent")
        } catch {
            print("Failed to save student: \(error)")
        }
    }
    
    func deleteStudentUserDefault(student: Student) -> Student? {
        let key = "savedStudent"
        if UserDefaults.standard.object(forKey: key) != nil {
            UserDefaults.standard.removeObject(forKey: key)
        } else {
            print("No saved student data found for key \(key).")
        }
        
        return nil
    }

    
    func signInStudent(for student:[String:Any]) async throws -> Student{
        let jsonData = try JSONSerialization.data(withJSONObject: student, options: [])
        
        guard let url = baseURL else{
            throw FetchError.badResponse
        }
        
        let createStudentURL = url.appending(path: "api/student/register") // MARK: api/Student -> endpoint
        
        var request = URLRequest(url: createStudentURL)
        request.httpMethod = "POST" // MARK: Specify HTTP method (POST)
        request.setValue("application/json", forHTTPHeaderField: "Content-Type") // Set Content-Type header
        request.httpBody = jsonData // MARK: Sending json in the reques
        
        let (data, response) = try await URLSession.shared.data(for: request) // making a tuple
        
        guard let response = response as? HTTPURLResponse, response.statusCode == 200 else{ // this is gonna give us urlResponse
            throw FetchError.badResponse
        }
        
        let decoder = JSONDecoder()
        decoder.keyDecodingStrategy = .convertFromSnakeCase
        
        let studentObj = try decoder.decode(Student.self, from: data)
        
        saveStudentToUserDefaults(student: studentObj)

        return studentObj
    }
    
    func loginInStudent(for student:[String:Any]) async throws -> Student{
        let jsonData = try JSONSerialization.data(withJSONObject: student, options: [])
        
        guard let url = baseURL else{
            throw FetchError.badResponse
        }
        
        let createStudentURL = url.appending(path: "api/student/login") // MARK: api/Student -> endpoint
        
        var request = URLRequest(url: createStudentURL)
        request.httpMethod = "POST" // MARK: Specify HTTP method (POST)
        request.setValue("application/json", forHTTPHeaderField: "Content-Type") // Set Content-Type header
        request.httpBody = jsonData // MARK: Sending json in the request
        
        let (data, response) = try await URLSession.shared.data(for: request) // making a tuple
        
        guard let response = response as? HTTPURLResponse, response.statusCode == 200 else{ // this is gonna give us urlResponse
            throw FetchError.badResponse
        }
        
        let decoder = JSONDecoder()
        decoder.keyDecodingStrategy = .convertFromSnakeCase
        
        let studentObj = try decoder.decode(Student.self, from: data)
        
        saveStudentToUserDefaults(student: studentObj)

        return studentObj
    }
    
    
    
    
    // Course section
    
    
    
    func fetchCourse(for stID:Int) async throws -> [Course]{
            // Create a URL
        guard let url = baseURL else {
            throw FetchError.badResponse
        }
        
        let createStudentURL = url.appending(path: "api/course/\(stID)") // MARK: api/Student -> endpoint
        
        // TODO: Fetch data
        let (data, response) = try await URLSession.shared.data(from: createStudentURL) // making a tuple
        
        // TODO: Handle response
        guard let response = response as? HTTPURLResponse, response.statusCode == 200 else{ // this is gonna give us urlResponse
            throw FetchError.badResponse
        }
        
        let decoder = JSONDecoder()
        decoder.keyDecodingStrategy = .convertFromSnakeCase
        
        // TODO: Decode data
        let courseData = try decoder.decode([Course].self, from: data)
        
        print("I have the data")
        // TODO: Return data
        return courseData
    }
    
    func addCourse(){
        
    }
    
    private mutating func fetchStudentJson(){
        if let url = Bundle.main.url(forResource: "student", withExtension: "json"){
            do{
                let data = try Data(contentsOf: url)
                let decoder = JSONDecoder()
                decoder.keyDecodingStrategy = .convertFromSnakeCase
                self.student = try decoder.decode(Student.self, from: data)
            }catch{
                print("Error decoding JSON data: \(error)")
            }
        }
    }
    
//    private mutating func fetchCoursesJson(){
//        if let url = Bundle.main.url(forResource: "course", withExtension: "json"){
//            do{
//                let data = try Data(contentsOf: url)
//                let decoder = JSONDecoder()
//                decoder.keyDecodingStrategy = .convertFromSnakeCase
//                self.course = try decoder.decode(Course.self, from: data)
//            }catch{
//                print("Error decoding JSON data: \(error)")
//            }
//        }
//
//    }
    
}
