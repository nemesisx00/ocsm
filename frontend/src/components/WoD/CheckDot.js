import React from 'react'
import './CheckDot.css'

const CheckedClass = 'checked'

class CheckDot extends React.Component
{
	render()
	{
		return (
			<div className={`checkDot ${this.props.className} ${(this.props.checked ? ` ${CheckedClass}` : '')}`}
					onClick={() => this.props.onClick()}>
				<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 14">
					<circle cx="7" cy="7" r="7" />
				</svg>
			</div>
		)
	}
}

export default CheckDot
