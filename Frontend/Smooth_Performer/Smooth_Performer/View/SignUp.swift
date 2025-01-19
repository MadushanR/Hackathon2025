//
//  SignUp.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import SwiftUI

struct SignUp: View {
    let vm = SignUpVM()
    @Binding var showSLView:Bool
    @Binding var changeView: Bool
    @State var firstName = ""
    @State var lastName = ""
    @State var email = ""
    @State var password = ""
    @State var studentId = ""
    
    var body: some View {
        VStack {
            // MARK: Image
            Image("chatting")
                .resizable()
                .scaledToFit()
                .frame(maxHeight: 350)
                .clipShape(.rect(cornerRadius: 10))
                .padding(.horizontal, 30)
                .padding(.top, 7)
            
            // MARK: Sign-Up Title
            HStack {
                Text("Sign-Up")
                    .font(.largeTitle)
                    .fontWeight(.bold)
                    .padding(.top, 30)
                    .padding(.leading, 40)
                    .padding(.bottom, 14)
                Spacer()
            }
            
            // MARK: Name and Surname
            HStack {
                VStack(alignment: .leading) {
                    TextField("First Name", text: $firstName)
                        .padding()
                        .background(Color.white)
                        .cornerRadius(8)
                        .overlay(
                            RoundedRectangle(cornerRadius: 8)
                                .stroke(Color.black, lineWidth: 2)
                                .opacity(0.4)
                        )
                    if !vm.firstNameError.isEmpty {
                        Text(vm.firstNameError)
                            .font(.caption)
                            .foregroundColor(.red)
                    }
                }
                .padding(.horizontal)
                
                VStack(alignment: .leading) {
                    TextField("Last Name", text: $lastName)
                        .padding()
                        .background(Color.white)
                        .cornerRadius(8)
                        .overlay(
                            RoundedRectangle(cornerRadius: 8)
                                .stroke(Color.black, lineWidth: 2)
                                .opacity(0.4)
                        )
                    if !vm.lastNameError.isEmpty {
                        Text(vm.lastNameError)
                            .font(.caption)
                            .foregroundColor(.red)
                    }
                }
                .padding(.trailing, 20)
            }
            .padding(.horizontal)
            
            // MARK: Email
            VStack(alignment: .leading) {
                TextField("Email", text: $email)
                    .keyboardType(.emailAddress)
                    .autocorrectionDisabled()
                    .autocapitalization(.none)
                    .padding()
                    .background(Color.white)
                    .cornerRadius(8)
                    .overlay(
                        RoundedRectangle(cornerRadius: 8)
                            .stroke(Color.black, lineWidth: 2)
                            .opacity(0.4)
                    )
                    .padding(.horizontal, 30)
                    .padding(.top, 9)
                if !vm.emailError.isEmpty {
                    Text(vm.emailError)
                        .font(.caption)
                        .foregroundColor(.red)
                        .padding(.horizontal, 30)
                }
            }
            
            // MARK: Password
            VStack(alignment: .leading) {
                SecureField("Password", text: $password)
                    .autocorrectionDisabled()
                    .autocapitalization(.none)
                    .padding()
                    .background(Color.white)
                    .cornerRadius(8)
                    .overlay(
                        RoundedRectangle(cornerRadius: 8)
                            .stroke(Color.black, lineWidth: 2)
                            .opacity(0.4)
                    )
                    .padding(.horizontal, 30)
                    .padding(.top, 9)
                if !vm.passwordError.isEmpty {
                    Text(vm.passwordError)
                        .font(.caption)
                        .foregroundColor(.red)
                        .padding(.horizontal, 30)
                }
            }
            
            // MARK: Submit Button
            Button {
                if vm.validateFields(firstName: firstName, lastName: lastName, email: email, password: password) {
                    Task {
                        showSLView = await vm.getData(for: [
                            "firstName": firstName,
                            "lastName": lastName,
                            "gpa": 0,
                            "desiredGPA": 0,
                            "email": email,
                            "password": password
                        ])
                        
                    }
                }
            } label: {
                Text("Submit")
                    .font(.title3)
                    .fontWeight(.bold)
                    .frame(maxWidth: .infinity)
                    .padding()
                    .background(Color.orange)
                    .foregroundColor(.white)
                    .cornerRadius(10)
                    .padding(.horizontal, 30)
                    .padding(.top)
            }
            if !vm.commonError.isEmpty {
                Text(vm.commonError)
                    .font(.caption)
                    .foregroundColor(.red)
                    .padding(.horizontal, 30)
            }
            
            // MARK: Log-In Redirect
            HStack {
                Text("I have an account.")
                Button {
                    changeView.toggle()
                } label: {
                    Text("Log-in")
                        .foregroundColor(.blue)
                }
            }
            .padding()
            
            Spacer(minLength: 40)
        }
    }
    
}


#Preview {
    SignUp(showSLView: .constant(false), changeView: .constant(false))
}
