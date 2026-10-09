<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register_name.aspx.cs" Inherits="MyFirstdbApp.Add_Student" MasterPageFile="~/Site.Master" %>
<asp:Content runat="server" ContentPlaceHolderID="MainContent">

    <script src="sweetalert2@11.js"></script>

    <style>
        :root {
            --primary: #4f46e5;
            --primary-dark: #4338ca;
            --text: #1f2937;
            --muted: #6b7280;
            --border: #e5e7eb;
            --bg-soft: #f9fafb;
        }

        .page-wrap {
            max-width: 900px;
            margin: 30px auto;
            padding: 0 15px;
            color: var(--text);
        }

        .page-title {
            font-size: 26px;
            font-weight: 700;
            margin: 0 0 4px 0;
        }

        .page-subtitle {
            color: var(--muted);
            margin-bottom: 24px;
        }

        .card-box {
            background: #fff;
            border: 1px solid var(--border);
            border-radius: 14px;
            padding: 24px;
            margin-bottom: 24px;
            box-shadow: 0 4px 14px rgba(0, 0, 0, 0.05);
        }

        .card-box h3 {
            font-size: 17px;
            font-weight: 700;
            margin: 0 0 18px 0;
        }

        .field { margin-bottom: 18px; }

        .field label {
            display: block;
            font-size: 13px;
            font-weight: 600;
            margin-bottom: 6px;
        }

        .field .hint {
            font-size: 12px;
            color: var(--muted);
            margin-top: 4px;
        }

        .card-box .form-control {
            width: 100%;
            height: 44px;
            padding: 8px 14px;
            border: 1px solid var(--border);
            border-radius: 10px;
            background: var(--bg-soft);
            box-shadow: none;
            transition: all 0.2s;
        }

        .card-box .form-control:focus {
            outline: none;
            background: #fff;
            border-color: var(--primary);
            box-shadow: 0 0 0 3px rgba(79, 70, 229, 0.15);
        }

        .btn-save {
            display: inline-block;
            padding: 11px 28px;
            font-weight: 600;
            color: #fff !important;
            background: var(--primary);
            border: none;
            border-radius: 10px;
            cursor: pointer;
            transition: background 0.2s, transform 0.1s;
        }

        .btn-save:hover { background: var(--primary-dark); }
        .btn-save:active { transform: scale(0.98); }

        .btn-home { margin-left: 8px; }

        /* ===== RESPONSIVE (cellphone) ===== */
        @media (max-width: 600px) {
            .page-wrap { margin: 16px auto; padding: 0 12px; }
            .page-title { font-size: 22px; }
            .page-subtitle { margin-bottom: 16px; font-size: 14px; }

            .card-box { padding: 16px; border-radius: 12px; }
            .card-box .form-control { height: 46px; font-size: 16px; }

            .btn-save { display: block; width: 100%; margin: 10px 0 0 0; padding: 13px; }
            .btn-home { margin-left: 0; }
        }
    </style>

    <div class="page-wrap">

        <h2 class="page-title">Student Registration</h2>
        <div class="page-subtitle">Add a new student.</div>

        <!-- ADD STUDENT -->
        <div class="card-box">
            <h3>Add New Student</h3>

            <div class="field">
                <label>Fullname</label>
                <asp:TextBox ID="txtname" runat="server" CssClass="form-control" placeholder="Lastname, Firstname, Middlename "></asp:TextBox>
                <div class="hint">Example: Rosal, JohnPaul A.</div>
            </div>

            <div class="field">
                <label>Guardian Contact No#</label>
                <asp:TextBox ID="txtcontact" runat="server" CssClass="form-control" placeholder="09XX XXX XXXX"></asp:TextBox>
            </div>

            <asp:Button runat="server" ID="btnSave" Text="Save Student" CssClass="btn-save" OnClick="btnSave_Click" />
            <asp:Button runat="server" ID="btnHome" Text="Payment" CssClass="btn-save btn-home" OnClick="btnHome_Click" />
            <asp:Button runat="server" Text="Payment Status" CssClass="btn-save btn-home" PostBackUrl="~/paymentstatus.aspx" />
            <asp:Button runat="server" Text="Payment History" CssClass="btn-save btn-home" PostBackUrl="~/timedatepayment.aspx" />
            <asp:Button runat="server" ID="btnLogout" Text="Logout" CssClass="btn-save btn-home" OnClick="btnLogout_Click" />

        </div>

    </div>

</asp:Content>