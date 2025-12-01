namespace AzureFunctionPet.Constants
{
    public static class EmailTemplates
    {
        public const string EmployeeOnboarding = @"
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333333;
        }}
        .header {{
            background-color: #004080;
            color: white;
            padding: 10px;
            text-align: center;
        }}
        .content {{
            margin: 20px;
        }}
        .footer {{
            margin: 20px;
            font-size: 0.9em;
            color: #666666;
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #004080;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 20px;
        }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>Welcome to the Solvencia, {{EmployeeName}}!</h1>
    </div>
    <div class='content'>
        <p>Dear {{EmployeeName}},</p>
        <p>We are excited to have you join our team. Below are your onboarding details:</p>
        <ul>
            <li><strong>Employee Code:</strong> {{EmployeeCode}}</li>
            <li><strong>Department:</strong> {{Department}}</li>
            <li><strong>Joining Date:</strong> {{DateOfJoining}}</li>
        </ul>
        <p>Please log in to the HR portal to complete your onboarding tasks and review company policies.</p>
        <a class='button' href='https://hr.company.com/onboarding'>Complete Onboarding</a>
    </div>
    <div class='footer'>
        <p>Best regards,<br/>HR Team</p>
        <p>This is an automated message. Please do not reply directly to this email.</p>
    </div>
</body>
</html>";


public const string DocumentVerification = @"
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            line-height: 1.6;
            color: #333333;
        }}
        .header {{
            background-color: #004080;
            color: white;
            padding: 10px;
            text-align: center;
        }}
        .content {{
            margin: 20px;
        }}
        .footer {{
            margin: 20px;
            font-size: 0.9em;
            color: #666666;
        }}
        .status {{
            font-weight: bold;
            color: {{StatusColor}};
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #004080;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            margin-top: 20px;
        }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>Document Verification Status</h1>
    </div>
    <div class='content'>
        <p>Dear {{EmployeeName}},</p>
        <p>The verification for your uploaded document has been completed. Here are the details:</p>
        <ul>
            <li><strong>Employee ID:</strong> {{EmployeeId}}</li>
            <li><strong>Document Name:</strong> {{FileName}}</li>
            <li><strong>Status:</strong> <span class='status'>{{Status}}</span></li>
            <li><strong>Message:</strong> {{Message}}</li>
            <li><strong>Verified On:</strong> {{VerifiedDate}}</li>
        </ul>
        <p>If you have any questions, please contact your HR department.</p>
        <a class='button' href='https://hr.company.com/documents'>View Documents</a>
    </div>
    <div class='footer'>
        <p>Best regards,<br/>HR Team</p>
        <p>This is an automated message. Please do not reply directly to this email.</p>
    </div>
</body>
</html>";
        public const string BirthdayWishes = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Happy Birthday!</title>
    <link href=""https://fonts.googleapis.com/css2?family=Montserrat:wght@400;500;600;700&family=Playfair+Display:wght@400;700&display=swap"" rel=""stylesheet"">
    <style>
        /* Reset and Base Styles */
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body { font-family: 'Montserrat', sans-serif; background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%); margin: 0; padding: 20px; }
        
        /* Email Container */
        .email-container { max-width: 600px; margin: 0 auto; background: white; border-radius: 24px; overflow: hidden; box-shadow: 0 20px 60px rgba(0, 0, 0, 0.1); }
        
        /* Header Section */
        .header { background: linear-gradient(135deg, #FF6B6B 0%, #FF8E53 100%); padding: 40px 20px; text-align: center; position: relative; }
        .header::before { content: ''; position: absolute; top: 0; left: 0; right: 0; height: 60px; background: url('data:image/svg+xml;utf8,<svg xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 1440 320""><path fill=""%23ffffff"" fill-opacity=""0.1"" d=""M0,96L48,112C96,128,192,160,288,186.7C384,213,480,235,576,213.3C672,192,768,128,864,128C960,128,1056,192,1152,208C1248,224,1344,192,1392,176L1440,160L1440,0L1392,0C1344,0,1248,0,1152,0C1056,0,960,0,864,0C768,0,672,0,576,0C480,0,384,0,288,0C192,0,96,0,48,0L0,0Z""></path></svg>'); }
        .confetti { position: absolute; width: 100%; height: 100%; top: 0; left: 0; pointer-events: none; }
        
        /* Birthday Icon */
        .birthday-icon { background: rgba(255, 255, 255, 0.2); border-radius: 50%; width: 120px; height: 120px; margin: 0 auto 25px; display: flex; align-items: center; justify-content: center; backdrop-filter: blur(10px); border: 3px solid rgba(255, 255, 255, 0.3); }
        .birthday-icon span { font-size: 48px; }
        
        /* Typography */
        h1 { font-family: 'Playfair Display', serif; font-size: 42px; font-weight: 700; color: white; margin-bottom: 15px; text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2); }
        .subtitle { font-size: 18px; color: rgba(255, 255, 255, 0.9); font-weight: 500; }
        
        /* Content Section */
        .content { padding: 50px 40px; }
        .greeting { font-family: 'Playfair Display', serif; font-size: 32px; color: #2D3436; text-align: center; margin-bottom: 30px; line-height: 1.3; }
        .greeting .name { color: #FF6B6B; font-weight: 700; }
        
        /* Message Cards */
        .message-card { background: #F8F9FA; border-radius: 16px; padding: 25px; margin: 25px 0; border-left: 4px solid #FF6B6B; }
        .message-card p { font-size: 17px; line-height: 1.6; color: #495057; margin-bottom: 15px; }
        .message-card:last-child p { margin-bottom: 0; }
        
        /* Celebration Section */
        .celebration { text-align: center; margin: 40px 0; padding: 30px; background: linear-gradient(135deg, #E3F2FD 0%, #F3E5F5 100%); border-radius: 20px; }
        .emoji-row { font-size: 48px; margin: 20px 0; letter-spacing: 15px; }
        .highlight-box { display: inline-block; background: linear-gradient(135deg, #4ECDC4 0%, #44A08D 100%); color: white; padding: 18px 45px; border-radius: 50px; font-size: 22px; font-weight: 700; box-shadow: 0 10px 30px rgba(68, 160, 141, 0.3); }
        
        /* Stats Section */
        .stats { display: flex; justify-content: space-around; margin: 40px 0; }
        .stat-item { text-align: center; }
        .stat-number { font-size: 36px; font-weight: 700; color: #FF6B6B; margin-bottom: 10px; }
        .stat-label { font-size: 14px; color: #6C757D; text-transform: uppercase; letter-spacing: 1px; }
        
        /* Footer */
        .footer { background: #2D3436; padding: 40px; text-align: center; }
        .team-name { font-family: 'Playfair Display', serif; font-size: 28px; color: white; margin-bottom: 15px; font-weight: 700; }
        .signature { font-size: 16px; color: rgba(255, 255, 255, 0.7); margin-bottom: 30px; }
        .social-icons { margin: 30px 0; }
        .social-icons a { display: inline-block; margin: 0 15px; width: 40px; height: 40px; background: rgba(255, 255, 255, 0.1); border-radius: 50%; text-align: center; line-height: 40px; color: white; text-decoration: none; transition: all 0.3s ease; }
        .social-icons a:hover { background: #FF6B6B; transform: translateY(-3px); }
        .copyright { font-size: 14px; color: rgba(255, 255, 255, 0.5); margin-top: 30px; }
        
        /* Decorative Elements */
        .decoration { text-align: center; margin: 30px 0; }
        .balloon { display: inline-block; width: 60px; height: 80px; background: linear-gradient(135deg, #FF6B6B 0%, #FF8E53 100%); border-radius: 50% 50% 50% 50% / 60% 60% 40% 40%; margin: 0 10px; position: relative; animation: float 3s ease-in-out infinite; }
        .balloon::after { content: ''; position: absolute; bottom: -20px; left: 50%; transform: translateX(-50%); width: 2px; height: 40px; background: #2D3436; }
        .balloon:nth-child(2) { background: linear-gradient(135deg, #4ECDC4 0%, #44A08D 100%); animation-delay: 0.5s; }
        .balloon:nth-child(3) { background: linear-gradient(135deg, #FFD166 0%, #FF9E6D 100%); animation-delay: 1s; }
        
        /* Animations */
        @keyframes float {
            0%, 100% { transform: translateY(0); }
            50% { transform: translateY(-15px); }
        }
        
        /* Responsive Design */
        @media (max-width: 600px) {
            .content { padding: 30px 20px; }
            h1 { font-size: 32px; }
            .greeting { font-size: 24px; }
            .stats { flex-direction: column; }
            .stat-item { margin: 15px 0; }
            .highlight-box { padding: 15px 30px; font-size: 18px; }
        }
    </style>
</head>
<body>
    <div class=""email-container"">
        
        <!-- Header with Confetti Effect -->
        <div class=""header"">
            <div class=""confetti""></div>
            <div class=""birthday-icon"">
                <span>🎂</span>
            </div>
            <h1>Happy Birthday!</h1>
            <p class=""subtitle"">Today is your special day!</p>
        </div>
        
        <!-- Main Content -->
        <div class=""content"">
            <h2 class=""greeting"">Dear <span class=""name"">{{Name}}</span>,</h2>
            
            <!-- Decorative Balloons -->
            <div class=""decoration"">
                <div class=""balloon""></div>
                <div class=""balloon""></div>
                <div class=""balloon""></div>
            </div>
            
            <!-- Personal Message -->
            <div class=""message-card"">
                <p>On this wonderful day, we want to take a moment to celebrate you! Your presence makes our workplace brighter and your contributions make our team stronger.</p>
                <p>We hope your birthday is filled with joy, laughter, and wonderful surprises!</p>
            </div>
            
            <!-- Celebration Time -->
            <div class=""celebration"">
                <div class=""emoji-row"">✨ 🎉 🥳 ✨</div>
                <div class=""highlight-box"">Make a Wish! 🎂</div>
                <div class=""emoji-row"">✨ 🎊 🎁 ✨</div>
            </div>
            
            <!-- Fun Stats -->
            <div class=""stats"">
                <div class=""stat-item"">
                    <div class=""stat-number"">24</div>
                    <div class=""stat-label"">Hours of Fun</div>
                </div>
                <div class=""stat-item"">
                    <div class=""stat-number"">365</div>
                    <div class=""stat-label"">Days Awesome</div>
                </div>
                <div class=""stat-item"">
                    <div class=""stat-number"">∞</div>
                    <div class=""stat-label"">Happy Memories</div>
                </div>
            </div>
            
            <!-- Final Message -->
            <div class=""message-card"" style=""background: linear-gradient(135deg, #FFF9C4 0%, #FFECB3 100%); border-left-color: #FFB300;"">
                <p style=""font-size: 18px; font-weight: 600; color: #5D4037;"">Here's to another year of amazing achievements, personal growth, and endless happiness!</p>
            </div>
        </div>
        
        <!-- Footer -->
        <div class=""footer"">
            <div class=""team-name"">Your Team Family</div>
            <p class=""signature"">With warmest wishes on your special day</p>
            
            <!-- Social Icons (Optional) -->
            <div class=""social-icons"">
                <a href=""#"">🎈</a>
                <a href=""#"">🎁</a>
                <a href=""#"">🎂</a>
                <a href=""#"">🎉</a>
            </div>
            
            <p class=""copyright"">© 2024 Company Name. All rights reserved.<br>
            Sent with ❤️ from the Birthday Notification System</p>
        </div>
    </div>

    <!-- Confetti Effect Script (CSS only) -->
    <style>
        .confetti:before, .confetti:after,
        .confetti i:before, .confetti i:after {
            content: '';
            position: absolute;
            width: 10px; height: 10px;
            background: white;
            border-radius: 50%;
            animation: confettiFall 5s linear infinite;
        }
        
        @keyframes confettiFall {
            0% { transform: translateY(-100px) rotate(0deg); opacity: 1; }
            100% { transform: translateY(500px) rotate(720deg); opacity: 0; }
        }
        
        /* Create multiple confetti pieces */
        .confetti:before { left: 10%; animation-delay: 0s; }
        .confetti:after { left: 20%; animation-delay: 1s; background: #FFD166; }
        .confetti i:before { left: 40%; animation-delay: 2s; background: #4ECDC4; }
        .confetti i:after { left: 60%; animation-delay: 3s; background: #FF8E53; }
    </style>
</body>
</html>";

    }

}
