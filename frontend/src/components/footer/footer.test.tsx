import { render, screen } from '@testing-library/react';
import Footer from './footer';

test('renders the footer text', () => {
  render(<Footer />)
  const footerElement = screen.getByText(/Human We Source/i)
  expect(footerElement).toBeInTheDocument()
})
