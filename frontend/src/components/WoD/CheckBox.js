import React from 'react'
import './CheckBox.css'

const CrossPath = 'M0,0 L12,12 M12,0 L0,12 Z'

class CheckBox extends React.Component
{
	render()
	{
		return (
			<div className="checkbox"
				onClick={() => this.props.clickHandler()}>
				<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 12 12">
					<path d={this.props.checked ? CrossPath : ''} />
				</svg>
			</div>
		)
	}
}

export default CheckBox
