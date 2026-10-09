<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="myfirst_sql_practice.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Login</title>
    <webopt:bundlereference runat="server" path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

    <script src="sweetalert2@11.js"></script>

    <style>
        :root {
            --text: #111111;
            --muted: #8a8a8a;
            --border: #dcdcdc;
            --bg: #ffffff;
            --accent: #111111;
        }

        * { box-sizing: border-box; }

        body {
            margin: 0;
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            background: var(--bg);
            font-family: "Segoe UI", Tahoma, Arial, sans-serif;
            color: var(--text);
        }

        .login-card {
            width: 100%;
            max-width: 560px;
            margin: 24px;
            padding: 56px 56px 48px;
            background: var(--bg);
            border: 1px solid var(--border);
            border-radius: 4px;
        }

        .login-card h2 {
            margin: 0 0 40px;
            font-size: 28px;
            font-weight: 600;
            letter-spacing: -0.5px;
        }

        .field { margin-bottom: 28px; }

        .field label {
            display: block;
            font-size: 12px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 1px;
            color: var(--muted);
            margin-bottom: 8px;
        }

        .field .form-control {
            display: block;
            width: 100%;
            max-width: 100%;
            padding: 12px 0;
            font-size: 16px;
            color: var(--text);
            background: transparent;
            border: none;
            border-bottom: 1px solid var(--border);
            border-radius: 0;
            box-shadow: none;
            outline: none;
        }

        .field .form-control:focus {
            border-bottom-color: var(--accent);
            box-shadow: none;
        }

        .field .form-control::placeholder { color: #bdbdbd; }

        .field .hint {
            font-size: 12px;
            color: var(--muted);
            margin-top: 8px;
        }

        .btn-save {
            display: block;
            width: 100%;
            margin-top: 40px;
            padding: 14px 24px;
            font-size: 15px;
            font-weight: 600;
            color: #fff !important;
            background: var(--accent);
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }

        /* ===== RESPONSIVE (cellphone) ===== */
        @media (max-width: 600px) {
            body { align-items: flex-start; }

            .login-card { margin: 16px; padding: 32px 22px 28px; }
            .login-card h2 { font-size: 24px; margin-bottom: 28px; }

            .field { margin-bottom: 22px; }
            .field .form-control { font-size: 16px; }

            .btn-save { margin-top: 28px; padding: 15px 20px; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-card">
            <h2>Login</h2>

            <div class="field">
                <label>Username</label>
                <asp:TextBox ID="txtuser" runat="server" CssClass="form-control" placeholder="Enter your username"></asp:TextBox>
                <div class="hint">Example: RosalGwapo</div>
            </div>

            <div class="field">
                <label>Password</label>
                <asp:TextBox ID="txtpass" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter your password"></asp:TextBox>
                <div class="hint">Example: Rosal123</div>
            </div>

            <asp:Button runat="server" ID="btnSave" Text="Login" CssClass="btn-save" OnClick="btnSave_Click" />
            <asp:Button runat="server" ID="register_Account" Text="Register_student_account" CssClass="btn-save" style="margin-top:12px;" OnClick="register_Account_Click" />

        </div>
    </form>
</body>
</html>