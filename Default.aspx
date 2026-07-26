<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PitStop.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- ===== HERO SECTION ===== -->
    <section class="hero-section">
        <div class="hero-content">
            <span class="hero-badge">⚡ F1 IN SCHOOLS &middot; SMK BANDAR PUTRA</span>
            <h1 class="hero-title">RACE THROUGH<br /><span class="hero-title-accent">LEARNING.</span></h1>
            <p class="hero-subtitle">PitStop is the gamified task management platform built for F1 in Schools teams. Complete missions, earn XP, and track your team's journey to the podium.</p>
            <div class="hero-actions">
                <a href="~/Register.aspx" runat="server" class="btn-hero-primary">Join the Team &rsaquo;</a>
                <a href="#features" class="btn-hero-secondary">See Features</a>
            </div>
        </div>
    </section>

    <!-- ===== ABOUT SECTION ===== -->
    <section class="about-section">
        <div class="about-text">
            <span class="section-label">ABOUT PITSTOP</span>
            <h2 class="section-title">MORE THAN A PLATFORM.<br /><span class="section-title-accent">A RACE STRATEGY.</span></h2>
            <p class="about-paragraph">PitStop was built specifically for the F1 in Schools team at SMK Bandar Putra, Johor. The competition demands precision, collaboration, and daily discipline — so we built a platform that makes the grind feel like a game.</p>
            <p class="about-paragraph">Students earn XP, maintain streaks, and unlock badges as they complete their roles. Advisors monitor progress in real time and assign personalised tasks — all backed by a structured database with full CRUD operations.</p>
            <ul class="about-tags">
                <li>🛡 Secure Login</li>
                <li>🏅 Achievement Badges</li>
                <li>🎯 Daily Missions</li>
            </ul>
        </div>
        <div class="about-media">
            <div class="about-image-placeholder"></div>
            <div class="achievement-toast">
                <span class="achievement-icon">⭐</span>
                <div>
                    <p class="achievement-title">Achievement Unlocked</p>
                    <p class="achievement-desc">First Submission +50XP</p>
                </div>
            </div>
        </div>
    </section>

<!-- ===== FEATURES SECTION ===== -->
<section class="features-section" id="features">
    <span class="section-label center">PLATFORM FEATURES</span>
    <h2 class="section-title center">EVERYTHING YOUR TEAM NEEDS <span class="section-title-accent">TO WIN.</span></h2>

    <div class="features-tabs">
        <span class="tab-label">Task Management</span>
        <span class="tab-label">XP &amp; Gamification</span>
        <span class="tab-label">Progress Tracking</span>
    </div>

    <div class="features-grid">
        <div class="feature-card active-card">
            <div class="feature-icon">✅</div>
            <span class="feature-label">FOR STUDENTS</span>
            <h3 class="feature-title">Task Management</h3>
            <p class="feature-desc">Advisors assign role-specific tasks to each team member. Students tick off daily goals, submit deliverables, and stay accountable throughout the competition season.</p>
            <a href="#" class="feature-link">Learn more &rsaquo;</a>
        </div>

        <div class="feature-card">
            <div class="feature-icon icon-orange">⚡</div>
            <span class="feature-label accent">GAME ON</span>
            <h3 class="feature-title">XP &amp; Gamification</h3>
            <p class="feature-desc">Earn experience points for every completed task. Unlock achievement badges, maintain daily streaks, and climb the team leaderboard to stay motivated.</p>
            <a href="#" class="feature-link accent">Learn more &rsaquo;</a>
        </div>

        <div class="feature-card">
            <div class="feature-icon">📈</div>
            <span class="feature-label">FOR ADVISORS</span>
            <h3 class="feature-title">Progress Tracking</h3>
            <p class="feature-desc">Advisors get a real-time dashboard to monitor each student's XP, submission history, and task completion — all in one place, always up to date.</p>
            <a href="#" class="feature-link">Learn more &rsaquo;</a>
        </div>
    </div>
</section>

    <!-- ===== HOW IT WORKS SECTION ===== -->
    <section class="steps-section" id="how-it-works">
        <span class="section-label center">GETTING STARTED</span>
        <h2 class="section-title center">THREE STEPS TO <span class="section-title-accent-orange">THE GRID.</span></h2>

        <div class="steps-grid">
            <div class="step-item">
                <div class="step-number">01</div>
                <h3 class="step-title">Register Your Account</h3>
                <p class="step-desc">Students and advisors sign up with their school credentials. Roles are assigned automatically — students get their dashboard, advisors get full team oversight.</p>
            </div>
            <div class="step-item">
                <div class="step-number">02</div>
                <h3 class="step-title">Get Your Tasks</h3>
                <p class="step-desc">Advisors assign personalised daily tasks based on each student's role — aerodynamics, design, engineering, or marketing. Clear goals, every day.</p>
            </div>
            <div class="step-item">
                <div class="step-number">03</div>
                <h3 class="step-title">Complete &amp; Earn XP</h3>
                <p class="step-desc">Submit your work, tick off tasks, and watch your XP grow. Unlock badges as you hit milestones. The more you do, the higher you climb.</p>
            </div>
        </div>
    </section>

    <!-- ===== CTA SECTION ===== -->
    <section class="cta-section">
        <span class="cta-badge">🏆 SEASON 2025 — NOW OPEN</span>
        <h2 class="cta-title">READY TO <span class="cta-title-accent">RACE?</span></h2>
        <p class="cta-subtitle">Register your account and start earning XP. Every task brings your team one step closer to the national stage.</p>
        <div class="cta-actions">
            <a href="~/Register.aspx" runat="server" class="btn-hero-primary">Create Account &rsaquo;</a>
            <a href="~/Login.aspx" runat="server" class="btn-hero-secondary">Member Login</a>
        </div>
    </section>

</asp:Content>
