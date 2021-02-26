import React from 'react'
import './CheckDot.css'

const CheckedClass = 'checked'

class CheckDot extends React.Component
{
	constructor(props)
	{
		super(props)
		this.state = {
			checked: props.checked
		}
	}
	
	render()
	{
		return (
			<div className={'checkDot' + (this.state.checked ? ` ${CheckedClass}` : '')}
					onClick={() => this.toggleCheckedState()}>
				<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 14">
					<circle cx="7" cy="7" r="7" />
				</svg>
			</div>
		)
	}
	
	toggleCheckedState()
	{
		this.setState((state) => {
			return { checked: !state.checked }
		})
	}
}

export default CheckDot
