<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaymentForm.aspx.cs" Inherits="myfirst_sql_practice.PaymentForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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
            max-width: 520px;
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
            padding: 28px;
            margin-bottom: 24px;
            box-shadow: 0 4px 14px rgba(0, 0, 0, 0.05);
        }

        .section-title {
            font-size: 18px;
            font-weight: 700;
            margin: 0 0 14px 0;
        }

        .field { margin-bottom: 18px; }

        .field label {
            display: block;
            font-size: 13px;
            font-weight: 600;
            margin-bottom: 6px;
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

        .amount-wrap { position: relative; }

        .amount-wrap .currency {
            position: absolute;
            left: 14px;
            top: 11px;
            font-weight: 700;
            color: var(--muted);
        }

        .amount-wrap .form-control { padding-left: 32px; }

        .btn-submit {
            width: 100%;
            padding: 12px;
            font-weight: 600;
            font-size: 15px;
            color: #fff !important;
            background: var(--primary);
            border: none;
            border-radius: 10px;
            cursor: pointer;
            transition: background 0.2s, transform 0.1s;
        }

        .btn-submit:hover { background: var(--primary-dark); }
        .btn-submit:active { transform: scale(0.98); }

        .btn-home { margin-top: 10px; }

        /* Payment Records grid */
        .table-wrap {
            overflow: auto;
            max-height: 380px;
            border: 1px solid var(--border);
            border-radius: 12px;
        }

        .modern-table {
            width: 100%;
            border-collapse: collapse;
            background: #fff;
            font-size: 13px;
        }

        .modern-table th {
            background: var(--bg-soft);
            color: var(--muted);
            font-size: 11px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            padding: 10px 12px;
            text-align: left;
            border-bottom: 1px solid var(--border);
            position: sticky;
            top: 0;
        }

        .modern-table td {
            padding: 10px 12px;
            border-bottom: 1px solid var(--border);
        }

        .modern-table tr:hover td { background: #f5f5ff; }

        .modern-table .grid-footer td {
            background: #eef2ff;
            color: #3730a3;
            font-weight: 700;
            border-top: 2px solid var(--primary);
            border-bottom: none;
        }

        .modern-table .grid-footer:hover td { background: #eef2ff; }

        .empty-msg {
            padding: 24px;
            text-align: center;
            color: var(--muted);
        }

        /* ===== RESPONSIVE (cellphone) ===== */
        @media (max-width: 600px) {
            .page-wrap { margin: 16px auto; padding: 0 12px; }
            .page-title { font-size: 22px; }
            .page-subtitle { margin-bottom: 16px; font-size: 14px; }

            .card-box { padding: 18px; border-radius: 12px; }
            .card-box .form-control { height: 46px; font-size: 16px; }
            .amount-wrap .currency { top: 13px; }

            .btn-submit { padding: 14px; }

            .table-wrap { max-height: 320px; -webkit-overflow-scrolling: touch; }
            .modern-table { min-width: 480px; }
            .modern-table td { white-space: nowrap; }
        }
    </style>

    <div class="page-wrap">

        <h2 class="page-title">Payment</h2>
        <div class="page-subtitle">Record a student's payment.</div>

        <!-- RECORD PAYMENT -->
        <div class="card-box">

            <div class="field">
                <label>Student</label>
                <asp:DropDownList ID="drpstudentlist" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="field">
                <label>Payment Type</label>
                <asp:DropDownList ID="drppaymentlist" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>

            <div class="field">
                <label>Amount</label>
                <div class="amount-wrap">
                    <span class="currency">₱</span>
                    <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" placeholder="0.00"></asp:TextBox>
                </div>
            </div>

            <div class="field">
                <label>Date &amp; Time Paid</label>
                <asp:TextBox ID="txtDatePaid" runat="server" CssClass="form-control" TextMode="DateTimeLocal"></asp:TextBox>
            </div>

            <asp:Button ID="Button1" runat="server" Text="Submit Payment" CssClass="btn-submit" OnClick="Button1_Click" />

            <asp:Button runat="server" Text="Register Name" CssClass="btn-submit btn-home" PostBackUrl="~/register_name.aspx" />

            <asp:Button runat="server" Text="Payment Status" CssClass="btn-submit btn-home" PostBackUrl="~/paymentstatus.aspx" />

            <asp:Button runat="server" Text="Payment History" CssClass="btn-submit btn-home" PostBackUrl="~/timedatepayment.aspx" />

            <asp:Button runat="server" ID="btnLogout" Text="Logout" CssClass="btn-submit btn-home" OnClick="btnLogout_Click" />

        </div>

        <!-- ADD NEW FEE -->
        <div class="card-box">
            <h3 class="section-title">Add New Fee</h3>

            <div class="field">
                <label>Fee Name</label>
                <asp:TextBox ID="txtFeeName" runat="server" CssClass="form-control" placeholder="e.g. Class Fund, Field Trip"></asp:TextBox>
            </div>

            <div class="field">
                <label>Fee Amount</label>
                <div class="amount-wrap">
                    <span class="currency">₱</span>
                    <asp:TextBox ID="txtFeeAmount" runat="server" CssClass="form-control" placeholder="0.00"></asp:TextBox>
                </div>
            </div>

            <asp:Button ID="btnAddFee" runat="server" Text="Add Fee" CssClass="btn-submit" OnClick="btnAddFee_Click" />
        </div>

        <!-- PAYMENT RECORDS -->
        <div class="card-box">
            <h3 class="section-title">Payment Records</h3>

            <div class="table-wrap">
                <asp:GridView runat="server" ID="GridViewPayments" CssClass="modern-table" AutoGenerateColumns="false" GridLines="None" BorderWidth="0"
                    ShowFooter="true" EmptyDataText="No payments recorded yet.">
                    <EmptyDataRowStyle CssClass="empty-msg" />
                    <FooterStyle CssClass="grid-footer" />
                    <Columns>
                        <asp:BoundField DataField="fullname" HeaderText="Student" />
                        <asp:BoundField DataField="fees" HeaderText="Fees" />
                        <asp:BoundField DataField="amountpay" HeaderText="Amount" DataFormatString="{0:N2}" HtmlEncode="false" />
                        <asp:BoundField DataField="Datepayed" HeaderText="Date &amp; Time" DataFormatString="{0:MMM dd, yyyy hh:mm tt}" HtmlEncode="false" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

    </div>

</asp:Content>