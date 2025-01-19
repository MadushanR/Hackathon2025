//
//  SignUpVM.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import Foundation

@Observable
class SignUpVM{
    enum FetchStatus{
        case notStarted
        case fetching
        case success
        case failed(message: String)
    }
    
    private(set) var status:FetchStatus = .notStarted
    
    private let fetcher = FetchService()
    
    var student: Student? = nil
    
    func getData(for stu:[String:Any]) async {
        status = .fetching
        print("Still fetching")
        
        do{
            student = try await fetcher.fetchStudent(for: stu)
            
            print("Fetched")
            
            status = .success
            print("success")
        }catch{
            status = .failed(message: "somthing went wrong!")
        }
    }

}
