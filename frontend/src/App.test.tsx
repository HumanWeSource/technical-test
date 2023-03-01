import { render, screen } from '@testing-library/react'
import App from './App'

test('renders the footer text', () => {
  render(<App />)
  const footerElement = screen.getByText(/Human We Source/i)
  expect(footerElement).toBeInTheDocument()
})
