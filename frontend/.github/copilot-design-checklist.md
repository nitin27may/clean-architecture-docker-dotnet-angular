# Design Quality Checklist

Use this checklist before committing any UI component:

## Visual Consistency
- [ ] Spacing uses 8px grid (4, 8, 16, 24, 32, 48, 64)
- [ ] Colors from theme palette (no hardcoded hex)
- [ ] Typography follows Material scale
- [ ] Consistent elevation (0, 1, 2, 4, 8)
- [ ] Border radius consistent (sm/md/lg)

## Responsiveness
- [ ] Works on mobile (< 600px)
- [ ] Works on tablet (600-960px)
- [ ] Works on desktop (> 960px)
- [ ] Touch targets minimum 44x44px
- [ ] Text readable at all sizes

## Theme Support
- [ ] Light theme perfect
- [ ] Dark theme perfect
- [ ] No hardcoded colors
- [ ] Proper contrast (WCAG AA)
- [ ] Icons/borders adapt

## Code Quality
- [ ] Standalone component
- [ ] OnPush change detection
- [ ] Signals for state
- [ ] New control flow syntax
- [ ] Proper TypeScript types
- [ ] No any types
- [ ] ARIA labels present

## Performance
- [ ] Lazy loading where appropriate
- [ ] Track by in loops
- [ ] No unnecessary re-renders
- [ ] Optimized imports