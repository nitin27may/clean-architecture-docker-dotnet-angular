---
layout: default
title: Roadmap
nav_order: 9
permalink: /roadmap
---

# Roadmap & Upcoming Features

We have an ambitious roadmap to enhance this project with the following features, organized into development phases. This document outlines our planned improvements and future direction.

## Phase 1: Enhanced RBAC & UI Modernization (✅ Completed)

**Focus**: Establishing a robust security foundation and creating a polished, modern UI.

### Role-Based Access Control Refinement
- [x] Configure distinct roles (Admin, Manager, Editor, Viewer)
- [x] Implement permission-based policies in backend
- [x] Create role assignment interface for administrators
- [x] Apply permission checks in UI components
- [x] Dynamic menu generation based on user roles
- [x] Role-based route guards implementation
- [x] UI screens for managing user roles and permissions

### User Management Improvements
- [x] Create comprehensive user management interface
- [x] Implement user creation and role assignment workflows
- [x] Add page operations and role mapping capabilities 
- [x] Fix issues with user profile management and permissions
- [x] Create API endpoints for all user management operations

### UI Enhancement with Tailwind CSS
- [x] Implement Tailwind CSS with custom theme
- [x] Create responsive layouts for all screen sizes
- [x] Design consistent component library
- [x] Add visual indicators for permission levels

### Contact Interface Improvements
- [x] Redesign contact list with modern data table
- [x] Create detailed contact profile view
- [x] Add categorization and organization features
- [x] Implement advanced filtering and search

## Phase 2: Activity Logging & Admin View (🔄 In Progress)

**Focus**: Implementing comprehensive audit tracking and administrative oversight.

### Enhanced Activity Logging
- [x] Extend existing audit logging functionality
- [x] Capture detailed context for all operations
- [ ] Implement logging categories for better organization
- [ ] Create storage and retrieval optimization

### Admin Activity Monitor Dashboard
- [ ] Design activity overview dashboard
- [ ] Implement filtering and search for logs
- [ ] Create visualization of recent system activity
- [ ] Add export capabilities for compliance purposes

## Phase 3: Real-Time Notifications

**Focus**: Adding collaborative features with real-time updates.

### SignalR Integration
- [ ] Implement SignalR hubs for contact updates
- [ ] Create client-side notification service
- [ ] Add connection management and resilience
- [ ] Implement user presence tracking

### Notification Center
- [ ] Design notification UI component
- [ ] Implement read/unread state management
- [ ] Create notification preferences
- [ ] Add notification history and archiving

### Browser Push Notifications
- [ ] Implement service worker registration in Angular
- [ ] Set up Web Push API integration in .NET backend
- [ ] Create VAPID key management system
- [ ] Develop notification subscription management
- [ ] Implement notification delivery when browser is closed
- [ ] Add notification interaction handling (clicks/dismissals)
- [ ] Create notification grouping and priority levels

## Phase 4: Application Logs & Monitoring

**Focus**: System-level logging, monitoring, and diagnostics.

### System Logging Implementation
- [ ] Create structured logging for application errors
- [ ] Implement performance tracking for critical operations
- [ ] Add security event logging
- [ ] Configure log levels and categories

### Admin Diagnostics Dashboard
- [ ] Design system health dashboard
- [ ] Create visualizations for error rates and performance
- [ ] Implement log search and filtering
- [ ] Add alert configuration for critical issues

### Performance Monitoring
- [ ] Add timing metrics for API endpoints
- [ ] Track database query performance
- [ ] Monitor client-side performance
- [ ] Implement threshold alerts

## Phase 5: Social Media Login

**Focus**: Enhancing authentication with external providers.

### OAuth Integration
- [ ] Implement Google authentication
- [ ] Add Microsoft authentication option
- [ ] Create secure token exchange
- [ ] Implement proper callback handling

### Account Linking
- [ ] Design account linking interface
- [ ] Implement identity merging logic
- [ ] Create profile synchronization
- [ ] Add preference for primary login method

## Phase 6: Advanced Features & Enhancements

**Focus**: Adding sophisticated features and workflows.

### Mobile Optimization
- [ ] Enhance mobile user experience
- [ ] Implement progressive web app (PWA) features
- [ ] Add offline capabilities
- [ ] Optimize for touch interactions

## Additional Planned Improvements

### PostgreSQL Integration (✅ Completed)
- [x] Update the Dapper helper to use PostgreSQL
- [x] Update existing queries to leverage PostgreSQL features
- [x] Implement PostgreSQL-specific optimizations

### DevOps Enhancements
- [ ] CI/CD pipeline improvements
- [ ] Kubernetes deployment support
- [ ] Infrastructure as Code (IaC) templates
- [ ] Automated testing in the pipeline

### Performance & Reliability
- [ ] Advanced caching strategies
- [ ] Background job processing
- [ ] Health checks and monitoring
- [ ] Rate limiting and API throttling

## Contributing to the Roadmap

We welcome community input on our roadmap priorities. If you have suggestions or would like to contribute to a specific feature, please:

1. Check our [issue tracker](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/issues) for related issues
2. Submit a [feature request](https://github.com/nitin27may/clean-architecture-docker-dotnet-angular/issues/new?assignees=&labels=&projects=&template=feature_request.md&title=) with details
3. Consider [contributing](../CONTRIBUTING.md) to the development effort

This roadmap is subject to change based on community feedback and evolving priorities.
