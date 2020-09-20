Imports AuthGuard
Imports System.IO
Imports System.Security.Cryptography
Imports System.Windows
Module Module1

    Sub Main()
        isValidDLL()
        'Detect if your application is running in a Virual Machine / Sandboxie...
        'Anti_Analysis.Init()
        'This connects your file to the AuthGuard.net API, and sends back your application settings and such
        Guard.Initialize("PROGRAMSECRET", "VERSION", "VARIABLESECRET")

        If GuardSettings.Freemode Then
            'Usually when your application doesn't need a login and has freemode enabled you put the code here you want to do
            MessageBox.Show("Freemode is active, bypassing login!", GuardSettings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Information)
        End If

home:
        PrintLogo()
        Console.WriteLine("[1] Register")
        Console.WriteLine("[2] Login")
        Console.WriteLine("[3] Extend Subscription")
        Dim choice As String = Console.ReadLine()

        If Equals(choice, "1") Then
reregister:
            Console.Clear()
            PrintLogo()
            Console.ForegroundColor = ConsoleColor.Yellow
            Console.WriteLine("***************************************************")
            Console.WriteLine("Register:")
            Console.WriteLine("***************************************************")
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine()
            Console.WriteLine("Username:")
            Dim username As String = Console.ReadLine()
            Console.WriteLine("Password:")
            Dim password As String = Console.ReadLine()
            Console.WriteLine("Email:")
            Dim email As String = Console.ReadLine()
            Console.WriteLine("License:")
            Dim license As String = Console.ReadLine()

            If Guard.Register(username, password, email, license) Then
                MessageBox.Show("You have successfully registered!", GuardSettings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Information)
                ' Do code of what you want after successful register here!
                Console.Clear()
                GoTo home
            Else
                GoTo reregister
            End If 'Retry
        ElseIf Equals(choice, "2") Then
relogin:
            Console.Clear()
            PrintLogo()
            Console.ForegroundColor = ConsoleColor.Yellow
            Console.WriteLine("***************************************************")
            Console.WriteLine("Login:")
            Console.WriteLine("***************************************************")
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine("Username:")
            Dim username As String = Console.ReadLine()
            Console.WriteLine("Password:")
            Dim password As String = Console.ReadLine()

            If Guard.Login(username, password) Then
                MessageBox.Show("You have successfully logged in!", UserInfo.Username, MessageBoxButton.OK, MessageBoxImage.Information)
                Console.Clear()
                PrintLogo()
                ' Success login stuff goes here
                Console.ForegroundColor = ConsoleColor.White
                Console.WriteLine("***************************************************")
                Console.WriteLine("All user information:")
                Console.WriteLine("***************************************************")
                Console.ForegroundColor = ConsoleColor.Cyan
                Console.WriteLine($"Username -> {UserInfo.Username}")
                Console.WriteLine($"Email -> {UserInfo.Email}")
                Console.WriteLine($"HWID -> {UserInfo.HWID}")
                Console.WriteLine($"User Level -> {UserInfo.Level}")
                Console.WriteLine($"User IP -> {UserInfo.IP}")
                Console.WriteLine($"Expiry -> {UserInfo.Expires}")
                'Put variable name here with the name of the variable in your panel - https://i.imgur.com/W7yl3MH.png
                Console.WriteLine($"Variable -> {Guard.Var("VARIABLENAME")}")
            Else
                GoTo relogin
            End If 'Retry
        ElseIf Equals(choice, "3") Then
reextend:
            Console.Clear()
            Console.ForegroundColor = ConsoleColor.Yellow
            Console.WriteLine("***************************************************")
            Console.WriteLine("Extend Subscription:")
            Console.WriteLine("***************************************************")
            Console.ForegroundColor = ConsoleColor.White
            Console.WriteLine()
            Console.WriteLine("Username:")
            Dim username As String = Console.ReadLine()
            Console.WriteLine("Password:")
            Dim password As String = Console.ReadLine()
            Console.WriteLine("License:")
            Dim token As String = Console.ReadLine()

            If Guard.RedeemToken(username, password, token) Then
                MessageBox.Show("You have successfully extended your subscription!", GuardSettings.ProgramName, MessageBoxButton.OK, MessageBoxImage.Information)
                'Do code of what you want after successful extend here!
                Console.Clear()
                GoTo home
            Else
                GoTo reextend
            End If 'Retry
        End If

        Console.Read()
    End Sub
    Public Sub PrintLogo()
        Console.ForegroundColor = ConsoleColor.Cyan
        Console.WriteLine("_______        _________        _______         _______ _______ ______  ")
        Console.WriteLine("(  ___  |\     /\__   __|\     /(  ____ |\     /(  ___  (  ____ (  __  \ ")
        Console.WriteLine("| (   ) | )   ( |  ) (  | )   ( | (    \| )   ( | (   ) | (    )| (  \  )")
        Console.WriteLine("| (___) | |   | |  | |  | (___) | |     | |   | | (___) | (____)| |   ) |")
        Console.WriteLine("|  ___  | |   | |  | |  |  ___  | | ____| |   | |  ___  |     __| |   | |")
        Console.WriteLine("| (   ) | |   | |  | |  | (   ) | | \_  | |   | | (   ) | (\ (  | |   ) |")
        Console.WriteLine("| )   ( | (___) |  | |  | )   ( | (___) | (___) | )   ( | ) \ \_| (__/  )")
        Console.WriteLine("|/     \(_______)  )_(  |/     \(_______(_______|/     \|/   \__(______/ ")
        Console.WriteLine()
        Console.ForegroundColor = ConsoleColor.White
    End Sub
    Public Function isValidDLL()
        If CalculateMD5("AuthGuard.dll") <> "3647e818b7ecdaa87e5b2f99bed034f0" Then
            MessageBox.Show("Hash check failed. Exiting...", "AuthGuard", MessageBoxButton.OK, MessageBoxImage.Error)
            Process.GetCurrentProcess().Kill()
        End If
        Return True
    End Function
    Private Function CalculateMD5(ByVal filename As String) As String
        Using md = MD5.Create()

            Using stream = File.OpenRead(filename)
                Dim hash = md.ComputeHash(stream)
                Return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant()
            End Using
        End Using
    End Function
End Module
