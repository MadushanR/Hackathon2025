//
//  FetchData.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import Foundation

class FetchService{
    private enum FetchError: Error {
        case badResponse
    }
    
    init() {
        fetchStudentJson()
        fetchCoursesJson()
    }
    
    private(set) var student:Student?
    private(set) var course:Course?
    
    private let baseURL = URL(string: "okok")!
    
    func fetchUser() async throws -> Student{
        
        // TODO: Fetch data
        let (data, response) = try await URLSession.shared.data(from: baseURL) // making a tuple
        
        // TODO: Handle response
        guard let response = response as? HTTPURLResponse, response.statusCode == 200 else{ // this is gonna give us urlResponse
            throw FetchError.badResponse
        }
        
        let decoder = JSONDecoder()
        decoder.keyDecodingStrategy = .convertFromSnakeCase
        
        // TODO: Decode data
        let userData = try decoder.decode(Student.self, from: data)
        print(userData)
        
        print("I have the data")
        // TODO: Return data
        return userData
    }
    
    func fetchCourse(for jId:[String:Any]) async throws -> Course{
        let jsonData = try JSONSerialization.data(withJSONObject: jId, options: [])
            
            // Create a URL
        guard let url = URL(string: "") else {
            throw FetchError.badResponse
        }
        
        // Create a URLRequest
        var request = URLRequest(url: url)
        request.httpMethod = "POST" // Specify HTTP method (e.g., POST)
        request.setValue("application/json", forHTTPHeaderField: "Content-Type") // Set Content-Type header
        request.httpBody = jsonData
        
        // TODO: Fetch data
        let (data, response) = try await URLSession.shared.data(for: request) // making a tuple
        
        // TODO: Handle response
        guard let response = response as? HTTPURLResponse, response.statusCode == 200 else{ // this is gonna give us urlResponse
            throw FetchError.badResponse
        }
        
        let decoder = JSONDecoder()
        decoder.keyDecodingStrategy = .convertFromSnakeCase
        
        // TODO: Decode data
        let courseData = try decoder.decode(Course.self, from: data)
        print(courseData)
        
        print("I have the data")
        // TODO: Return data
        return courseData
    }
    
    private func fetchStudentJson(){
        if let url = Bundle.main.url(forResource: "student", withExtension: "json"){
            do{
                let data = try Data(contentsOf: url)
                let decoder = JSONDecoder()
                decoder.keyDecodingStrategy = .convertFromSnakeCase
                self.student = try decoder.decode(Student.self, from: data)
                print(student!)
            }catch{
                print("Error decoding JSON data: \(error)")
            }
        }
    }
    
    private func fetchCoursesJson(){
        if let url = Bundle.main.url(forResource: "course", withExtension: "json"){
            do{
                let data = try Data(contentsOf: url)
                let decoder = JSONDecoder()
                decoder.keyDecodingStrategy = .convertFromSnakeCase
                self.course = try decoder.decode(Course.self, from: data)
                print(course!)
                self.student?.courses?.append(course!)
                print(student!.courses![0])
            }catch{
                print("Error decoding JSON data: \(error)")
            }
        }

    }
    
}
