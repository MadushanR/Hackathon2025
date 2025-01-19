//
//  Root.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-19.
//

import SwiftUI

struct Root: View {
    @State private var IsUser = true
    var body: some View {
        ZStack{
            NavigationStack{
                HomeTab()
            }
        }
        .onAppear{
            let currentStudent = try? MainViewModel().getCurrentUser()
            self.IsUser = currentStudent == nil
        }
        .fullScreenCover(isPresented: $IsUser) {
            NavigationStack{
                LSTab()
            }
        }
    }
}

#Preview {
    Root()
}
