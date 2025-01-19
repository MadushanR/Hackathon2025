//
//  SignUpVM.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import Foundation

@Observable
class SignUpVM:ObservableObject{
    enum FetchStatus{
        case notStarted
        case fetching
        case success
        case failed(message: String)
    }
    
    var firstNameError: String = ""
    var lastNameError: String = ""
    var emailError: String = ""
    var passwordError: String = ""
    
    var commonError:String = ""
    
    private(set) var status:FetchStatus = .notStarted
    
    private let fetcher = FetchService()
    
    var student: Student? = nil
    
    func getData(for stu:[String:Any]) async -> Bool {
        status = .fetching
        print("Fetching from sign view model")
        
        do{
            print("Just before vm function")
            student = try await fetcher.signInStudent(for: stu)
            print("Fetched form signup VM")
            
            status = .success
            print("success From signup VM")
            return false
        }catch{
            commonError = "Email already exists"
            status = .failed(message: "Email already Exists")
            return true
        }
    }
    
    func validateFields(firstName: String, lastName: String, email: String, password: String) -> Bool {
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
            
            return isValid
        }

}
