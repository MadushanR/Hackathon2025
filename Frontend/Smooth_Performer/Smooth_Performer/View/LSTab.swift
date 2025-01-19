//
//  TabView.swift
//  Smooth_Performer
//
//  Created by Digaant Dogra on 2025-01-18.
//

import SwiftUI

struct LSTab: View {
    @State var login = true
    var body: some View {
        if login{
            Login(changeView: $login)
        }else{
            SignUp(changeView: $login)
        }
    }
}

#Preview {
    LSTab()
}
