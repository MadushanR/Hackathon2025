//
//  Login.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import SwiftUI

struct Login: View {
    @Binding var changeView:Bool
    @State var email = ""
    @State var password = ""
    
    var body: some View {
        Image("coffee")
            .resizable()
            .scaledToFit()
            .frame(maxHeight: 350)
            .clipShape(.rect(cornerRadius: 10))
            .padding(.horizontal, 30)
            .padding(.top,7)
        
        HStack{
            Text("Log-In")
                .font(.largeTitle)
                .fontWeight(.bold)
                .padding(.top,30)
                .padding(.leading,40)
                .padding(.bottom,14)
            
            Spacer()
        }
        
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
            .padding()
            .autocorrectionDisabled()
            .autocapitalization(.none)
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
        }label: {
            Text("Login")
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
            Text("Don't have account?")
            Button{
                // MARK: Changing view
                changeView.toggle()
            }label: {
                Text("Create Account")
                    .foregroundColor(.blue)
            }
        }
        .padding()

        
        
        Spacer()
    }
}

#Preview {
    Login(changeView: .constant(true))
}
