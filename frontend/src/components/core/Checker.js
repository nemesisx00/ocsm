import React from 'react'
import {normalizeClassNames} from './Utilities'
import './Checker.css'

const CheckedClass = 'checked'

const CheckerType = Object.freeze({
	Circle: 'circle',
	Line: 'line'
})

const CheckerLineStatus = Object.freeze({
	None: 'none',
	Single: 'single',
	Double: 'double',
	Triple: 'triple'
})

const LinePath = Object.freeze({
	None: '',
	Single: 'M0,0 L12,12 Z',
	Double: 'M0,0 L12,12 M12,0 L0,12 Z',
	Triple: 'M0,0 L12,12 M12,0 L0,12 M6,0 L6,12 Z'
})

class Checker extends React.Component
{
	render()
	{
		switch(this.props.type)
		{
			case CheckerType.Circle:
				return this.renderCircle()
			default:
				return this.renderLine()
		}
	}
	
	renderCircle()
	{
		return (
			<div className={normalizeClassNames(`checker ${this.props.type} ${this.props.className ? this.props.className : ''} ${this.props.checked ? ` ${CheckedClass}` : ''}`)}
					onClick={() => this.props.clickHandler(this.props.checked)}>
				<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 14 14">
					<circle cx="7" cy="7" r="7" />
				</svg>
			</div>
		)
	}
	
	renderLine()
	{
		return (
			<div className={normalizeClassNames('checker')} onClick={() => this.props.clickHandler()}>
				<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 12 12">
					<path d={this.getPath()} />
				</svg>
			</div>
		)
	}
	
	getPath()
	{
		switch(this.props.lineStatus)
		{
			case CheckerLineStatus.Single:
				return LinePath.Single
			case CheckerLineStatus.Double:
				return LinePath.Double
			case CheckerLineStatus.Triple:
				return LinePath.Triple
			default:
				return LinePath.None
		}
	}
}

Checker.Types = CheckerType
Checker.LineStatus = CheckerLineStatus

export default Checker
