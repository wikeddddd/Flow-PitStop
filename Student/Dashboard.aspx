<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/MemberSection.master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="PitStop.Student.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MemberMainContent" runat="server">

    <h1 class="dashboard-greeting">Hi, <asp:Label ID="lblStudentName" runat="server" Text="Student"></asp:Label>! 👋</h1>
    <!-- TODO: [Teammate handling auth/data] bind lblStudentName to logged-in user -->

    <div class="stat-card">
        <div class="stat-header">
            <div class="stat-badge">🏆</div>
            <span class="stat-level">Level 4 Student</span>
            <span class="stat-progress-tag">1/9 tasks done</span>
            <!-- TODO: bind level + task count dynamically -->
        </div>
        <div class="stat-bar-row">
            <span class="stat-bar-label">HP</span>
            <div class="stat-bar-track"><div class="stat-bar-fill hp" style="width: 80%;"></div></div>
            <span class="stat-bar-value">80/100</span>
        </div>
        <div class="stat-bar-row">
            <span class="stat-bar-label">XP</span>
            <div class="stat-bar-track"><div class="stat-bar-fill xp" style="width: 48%;"></div></div>
            <span class="stat-bar-value">240/500</span>
        </div>
    </div>

    <div class="announce-card">
        <div class="card-heading">Latest Announcements</div>
        <div class="announce-item">
            <div class="announce-icon">📄</div>
            <div>
                <div class="announce-title">Turnitin System Outage</div>
                <div class="announce-sub">Web Application Programming</div>
                <div class="announce-date">17 April 2026, 2:27 PM</div>
            </div>
        </div>
        <!-- TODO: bind announcements list from database -->
    </div>

    <div class="section-heading">Timeline</div>
    <div class="timeline-header">
        <div class="timeline-filters">
            <span class="filter-pill active">Next 7 days</span>
            <span class="filter-pill">Next 30 days</span>
            <span class="filter-pill">All</span>
        </div>
    </div>

    <div class="timeline-date">TODAY</div>
    <div class="timeline-item">
        <div class="timeline-left">
            <span class="timeline-time">11:59</span>
            <div>
                <div class="timeline-title">Section One - Individual Submission</div>
                <div class="timeline-sub">Web Application Programming</div>
            </div>
        </div>
        <div class="timeline-right">
            <span class="xp-tag">+40 XP</span>
            <button class="btn-submit">Add Submission</button>
        </div>
    </div>
    <!-- TODO: bind task list from database, loop with Repeater/GridView -->

</asp:Content>