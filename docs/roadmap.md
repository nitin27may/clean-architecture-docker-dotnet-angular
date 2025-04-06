# Roadmap & Upcoming Features

We have an ambitious roadmap to enhance this project with the following features, organized into development phases. This document outlines our planned improvements and future direction.

## Phase 1: Enhanced RBAC & UI Modernization

**Focus**: Establishing a robust security foundation and creating a polished, modern UI.

### Role-Based Access Control Refinement
- [ ] Configure distinct roles (Admin, Manager, Editor, Viewer)
- [ ] Implement permission-based policies in backend
- [ ] Create role assignment interface for administrators
- [ ] Apply permission checks in UI components

### UI Enhancement with Tailwind CSS
- [ ] Implement Tailwind CSS with custom theme
- [ ] Create responsive layouts for all screen sizes
- [ ] Design consistent component library
- [ ] Add visual indicators for permission levels

### Contact Interface Improvements
- [ ] Redesign contact list with modern data table
- [ ] Create detailed contact profile view
- [ ] Add categorization and organization features
- [ ] Implement advanced filtering and search

## Phase 2: Activity Logging & Admin View

**Focus**: Implementing comprehensive audit tracking and administrative oversight.

### Enhanced Activity Logging
- [ ] Extend existing audit logging functionality
- [ ] Capture detailed context for all operations
- [ ] Implement logging categories for better organization
- [ ] Create storage and retrieval optimization

### Admin Activity Dashboard
- [ ] Design activity overview dashboard
- [ ] Implement filtering and search for logs
- [ ] Create visualization of recent system activity
- [ ] Add export capabilities for compliance purposes

### Contact-Level Activity Timeline
- [ ] Add activity history to contact profiles
- [ ] Show who changed what and when
- [ ] Implement filtering for contact-specific activities
- [ ] Display visual diff of contact changes

## Phase 3: Application Logs & Monitoring

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

## Phase 4: Real-Time Notifications

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

## Additional Planned Improvements

### PostgreSQL Integration
- [x] Update the Dapper helper to use PostgreSQL
- [x] Update existing queries to leverage PostgreSQL features
- [ ] Implement PostgreSQL-specific optimizations

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
