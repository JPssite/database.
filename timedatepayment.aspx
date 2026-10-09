<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="timedatepayment.aspx.cs" Inherits="myfirst_sql_practice.timedatepayment" %>
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
        .page-wrap { max-width: 900px; margin: 30px auto; padding: 0 15px; color: var(--text); }
        .page-title { font-size: 26px; font-weight: 700; margin: 0 0 4px 0; }
        .page-subtitle { color: var(--muted); margin-bottom: 24px; }
        .card-box { background: #fff; border: 1px solid var(--border); border-radius: 14px; padding: 24px; margin-bottom: 24px; box-shadow: 0 4px 14px rgba(0,0,0,0.05); }
        .card-box .form-control { width: 100%; height: 44px; padding: 8px 14px; border: 1px solid var(--border); border-radius: 10px; background: var(--bg-soft); box-shadow: none; }
        .btn-save { display: inline-block; padding: 11px 28px; font-weight: 600; color: #fff !important; background: var(--primary); border: none; border-radius: 10px; cursor: pointer; }
        .btn-save:hover { background: var(--primary-dark); }
        .btn-home { margin-left: 8px; }
        .filter-row { display: flex; align-items: center; gap: 12px; margin-bottom: 18px; }
        .filter-row label { font-weight: 600; font-size: 13px; margin: 0; white-space: nowrap; }
        .summary-box { background: var(--bg-soft); border: 1px solid var(--border); border-radius: 10px; padding: 12px 16px; margin-bottom: 18px; }
        .table-wrap { overflow-x: auto; border: 1px solid var(--border); border-radius: 12px; }
        .modern-table { width: 100%; border-collapse: collapse; background: #fff; }
        .modern-table th { background: var(--bg-soft); color: var(--muted); font-size: 12px; font-weight: 700; text-transform: uppercase; letter-spacing: 0.5px; padding: 12px 16px; text-align: left; border-bottom: 1px solid var(--border); }
        .modern-table td { padding: 12px 16px; border-bottom: 1px solid var(--border); }
        .modern-table tr:last-child td { border-bottom: none; }
        .modern-table tr:hover td { background: #f5f5ff; }
        .modern-table .grid-footer td { background: #eef2ff; color: #3730a3; font-weight: 700; border-top: 2px solid var(--primary); }
        .modern-table .grid-footer:hover td { background: #eef2ff; }
        .badge-count { display: inline-block; padding: 4px 12px; border-radius: 999px; font-size: 12px; font-weight: 700; background: #e0e7ff; color: #3730a3; }
        .empty-msg { padding: 30px; text-align: center; color: var(--muted); }

        /* ===== RESPONSIVE (cellphone) ===== */
        @media (max-width: 600px) {
            .page-wrap { margin: 16px auto; padding: 0 12px; }
            .page-title { font-size: 22px; }
            .page-subtitle { margin-bottom: 16px; font-size: 14px; }

            .card-box { padding: 16px; border-radius: 12px; }
            .card-box .form-control { height: 46px; font-size: 16px; }

            .filter-row { flex-direction: column; align-items: stretch; gap: 6px; }

            .btn-save { display: block; width: 100%; margin: 10px 0 0 0; padding: 13px; }
            .btn-home { margin-left: 0; }

            .table-wrap { -webkit-overflow-scrolling: touch; }
            .modern-table { min-width: 600px; font-size: 13px; }
            .modern-table th, .modern-table td { padding: 10px 12px; white-space: nowrap; }

            .summary-box { font-size: 13px; line-height: 1.6; }
        }
    </style>

    <div class="page-wrap">

        <h2 class="page-title">Payment History</h2>
        <div class="page-subtitle">Kailan nagbayad ang student, at ilang beses siyang nagbayad.</div>

        <div class="card-box">

            <div class="filter-row">
                <label>Student:</label>
                <asp:DropDownList runat="server" ID="drpStudent" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpStudent_SelectedIndexChanged"></asp:DropDownList>
            </div>

            <asp:Panel runat="server" ID="pnlSummary" CssClass="summary-box" Visible="false">
                <asp:Label runat="server" ID="lblSummary"></asp:Label>
            </asp:Panel>

            <div class="table-wrap">
                <asp:GridView runat="server" ID="GridViewHistory" CssClass="modern-table" AutoGenerateColumns="false" GridLines="None" BorderWidth="0"
                    ShowFooter="true" EmptyDataText="Select a student to view the payment history.">
                    <EmptyDataRowStyle CssClass="empty-msg" />
                    <FooterStyle CssClass="grid-footer" />
                    <Columns>
                        <asp:BoundField DataField="fullname" HeaderText="Fullname" />
                        <asp:BoundField DataField="fees" HeaderText="Fees" />
                        <asp:TemplateField HeaderText="Payment #">
                            <ItemTemplate>
                                <span class="badge-count"><%# Eval("paymentno") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="amountpay" HeaderText="Amount" DataFormatString="{0:N2}" HtmlEncode="false" />
                        <asp:BoundField DataField="Datepayed" HeaderText="Date &amp; Time Paid" DataFormatString="{0:MMM dd, yyyy  hh:mm tt}" HtmlEncode="false" />
                    </Columns>
                </asp:GridView>
            </div>

            <div style="margin-top:18px;">
                <asp:Button runat="server" ID="btnRegister" Text="Register" CssClass="btn-save" OnClick="btnRegister_Click" />
                <asp:Button runat="server" ID="btnPayment" Text="Payment" CssClass="btn-save btn-home" OnClick="btnPayment_Click" />
                <asp:Button runat="server" ID="btnStatus" Text="Payment Status" CssClass="btn-save btn-home" OnClick="btnStatus_Click" />
                <asp:Button runat="server" ID="btnLogout" Text="Logout" CssClass="btn-save btn-home" OnClick="btnLogout_Click" />
            </div>
        </div>

    </div>

</asp:Content>