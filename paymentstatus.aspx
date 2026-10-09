<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="paymentstatus.aspx.cs" Inherits="myfirst_sql_practice.paymentstatus" %>
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

        .filter-row {
            display: flex;
            align-items: center;
            gap: 12px;
            margin-bottom: 18px;
        }

        .filter-row label {
            font-weight: 600;
            font-size: 13px;
            margin: 0;
            white-space: nowrap;
        }

        .table-wrap {
            overflow-x: auto;
            border: 1px solid var(--border);
            border-radius: 12px;
        }

        .modern-table {
            width: 100%;
            border-collapse: collapse;
            background: #fff;
        }

        .modern-table th {
            background: var(--bg-soft);
            color: var(--muted);
            font-size: 12px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            padding: 12px 16px;
            text-align: left;
            border-bottom: 1px solid var(--border);
        }

        .modern-table td {
            padding: 12px 16px;
            border-bottom: 1px solid var(--border);
        }

        .modern-table tr:last-child td { border-bottom: none; }
        .modern-table tr:hover td { background: #f5f5ff; }

        .modern-table .grid-footer td {
            background: #eef2ff;
            color: #3730a3;
            font-weight: 700;
            border-top: 2px solid var(--primary);
        }

        .modern-table .grid-footer:hover td { background: #eef2ff; }

        .badge-status {
            display: inline-block;
            padding: 4px 12px;
            border-radius: 999px;
            font-size: 12px;
            font-weight: 700;
        }

        .badge-paid { background: #dcfce7; color: #166534; }
        .badge-unpaid { background: #fee2e2; color: #991b1b; }

        .empty-msg {
            padding: 30px;
            text-align: center;
            color: var(--muted);
        }

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
        }
    </style>

    <div class="page-wrap">

        <h2 class="page-title">Payment Status</h2>
        <div class="page-subtitle">View the students' payments per fee.</div>

        <div class="card-box">

            <div class="filter-row">
                <label>Fees:</label>
                <asp:DropDownList runat="server" ID="drpFees" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpFees_SelectedIndexChanged"></asp:DropDownList>
            </div>

            <div class="table-wrap">
                <asp:GridView runat="server" ID="GridViewStudentList" CssClass="modern-table" AutoGenerateColumns="false" GridLines="None" BorderWidth="0"
                    ShowFooter="true" EmptyDataText="Select a fee to view the student list.">
                    <EmptyDataRowStyle CssClass="empty-msg" />
                    <FooterStyle CssClass="grid-footer" />
                    <Columns>
                        <asp:BoundField DataField="fullname" HeaderText="Fullname" />
                        <asp:BoundField DataField="contactno" HeaderText="Contact Number" />
                        <asp:BoundField DataField="totalpaid" HeaderText="Total Paid" DataFormatString="{0:N2}" HtmlEncode="false" />
                        <asp:BoundField DataField="balance" HeaderText="Balance" DataFormatString="{0:N2}" HtmlEncode="false" />
                        <asp:TemplateField HeaderText="Remarks">
                            <ItemTemplate>
                                <span class='badge-status <%# Eval("remarks").ToString() == "PAID" ? "badge-paid" : "badge-unpaid" %>'>
                                    <%# Eval("remarks") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

            <div style="margin-top:18px;">
                <asp:Button runat="server" ID="btnRegister" Text="Register" CssClass="btn-save" OnClick="btnRegister_Click" />
                <asp:Button runat="server" ID="btnPayment" Text="Payment" CssClass="btn-save btn-home" OnClick="btnPayment_Click" />
                <asp:Button runat="server" ID="btntimedate" Text="Payment History" CssClass="btn-save btn-home" OnClick="btntimedate_Click" />
                <asp:Button runat="server" ID="btnLogout" Text="Logout" CssClass="btn-save btn-home" OnClick="btnLogout_Click" />
            </div>
        </div>

    </div>

</asp:Content>