//
//  SignUpVM.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import Foundation

class SignUpVM:ObservableObject{
    enum FetchStatus{
        case notStarted
        case fetching
        case success
        case failed(message: String)
    }
    
    @Published var firstNameError: String = ""
    @Published var lastNameError: String = ""
    @Published var emailError: String = ""
    @Published var passwordError: String = ""
    @Published var studentIdError: String = ""
    
    private(set) var status:FetchStatus = .notStarted
    
    private let fetcher = FetchService()
    
    var student: Student? = nil
    
    func getData(for stu:[String:Any]) async {
        status = .fetching
        print("Still fetching")
        
        do{
            student = try await fetcher.SignInStudent(for: stu)
            
            print("Fetched")
            
            status = .success
            print("success")
        }catch{
            status = .failed(message: "somthing went wrong!")
        }
    }
    
    func validateFields(firstName: String, lastName: String, email: String, password: String, studentId: String) -> Bool {
            var isValid = true
            
            // First Name Validation
            if firstName.isEmpty {
                firstNameError = "First name is required."
                isValid = false
            } else {
                firstNameError = ""
            }
            
            // Last Name Validation
            if lastName.isEmpty {
                lastNameError = "Last name is required."
                isValid = false
            } else {
                lastNameError = ""
            }
            
            // Email Validation
            if email.isEmpty {
                emailError = "Email is required."
                isValid = false
            } else if !email.contains("@") || !email.contains(".") {
                emailError = "Please enter a valid email address."
                isValid = false
            } else {
                emailError = ""
            }
            
            // Password Validation
            if password.isEmpty {
                passwordError = "Password is required."
                isValid = false
            } else if password.count < 6 {
                passwordError = "Password must be at least 6 characters."
                isValid = false
            } else {
                passwordError = ""
            }
            
            // Student ID Validation
            if studentId.isEmpty {
                studentIdError = "Student ID is required."
                isValid = false
            } else if Int(studentId) == nil {
                studentIdError = "Student ID must be a valid number."
                isValid = false
            } else {
                studentIdError = ""
            }
            
            return isValid
        }

}
