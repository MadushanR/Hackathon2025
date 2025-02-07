//
//  SignUp.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import SwiftUI

struct SignUp: View {
    let vm = SignUpVM()
    @Binding var changeView:Bool
    @State var firstName = ""
    @State var lastName = ""
    @State var email = ""
    @State var password = ""
    @State var studentId = ""
    
    var body: some View {
        Image("chatting")
            .resizable()
            .scaledToFit()
            .frame(maxHeight: 350)
            .clipShape(.rect(cornerRadius: 10))
            .padding(.horizontal, 30)
            .padding(.top,7)
        
        HStack{
            Text("Sign-Up")
                .font(.largeTitle)
                .fontWeight(.bold)
                .padding(.top,30)
                .padding(.leading,40)
                .padding(.bottom,14)
            
            Spacer()
        }
        
        // MARK: name and surname
        HStack{
            TextField("First Name", text: $firstName)
                .padding() // Adds padding inside the TextField
                .background(Color.white) // Background color
                .cornerRadius(8) // Rounds the corners
                .overlay( // Adds the outline
                    RoundedRectangle(cornerRadius: 8)
                        .stroke(Color.black, lineWidth: 2)
                        .opacity(0.4)// Outline color and width
                )
                .padding(.horizontal)
            TextField("Surname",text: $lastName)
                .padding()
                .background(Color.white)
                .cornerRadius(8)
                .overlay(
                    RoundedRectangle(cornerRadius: 8)
                        .stroke(Color.black, lineWidth: 2)
                        .opacity(0.4)
                )
                .padding(.trailing,20)
        }
        .padding(.horizontal)
        
        // MARK: Student Id
        TextField("Student ID",text: $studentId)
            .keyboardType(.numberPad)
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
            .padding(.horizontal,30)
            .padding(.top,9)
        
        // MARK: Email
        TextField("Email",text: $email)
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
            .padding(.horizontal,30)
            .padding(.top,9)
        
        // MARK: password
        SecureField("Password",text: $password)
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
            .padding(.horizontal,30)
            .padding(.top,9)
        
        Button{
            // TODO: function for sending email and password while getting 200
            Task{
                await vm.getData(for: [
                    "firstName":firstName,
                    "lastName":lastName,
                    "gpa":0,
                    "dGpa":0,
                    "id":studentId,
                    "email":email,
                    "password":password,
                    "courses":nil
                ])
            }
        }label: {
            Text("Submit")
                .font(.title3)
                .fontWeight(.bold)
                .frame(maxWidth: .infinity)
                .padding()
                .background(Color.orange)
                .foregroundColor(.white)
                .cornerRadius(10)
                .padding(.horizontal,30)
                .padding(.top)
        }
        
        HStack{
            Text("I have an account.")
            Button{
                // MARK: Changing view
                changeView.toggle()
            }label: {
                Text("Log-in")
                    .foregroundColor(.blue)
            }
        }
        .padding()
        
        
        Spacer(minLength: 40)
    }
}

#Preview {
    SignUp(changeView: .constant(false))
}
