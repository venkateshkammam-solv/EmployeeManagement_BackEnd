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

    }
}
